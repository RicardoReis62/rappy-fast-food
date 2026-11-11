USE RappyFastFoodDB;
GO

CREATE OR ALTER PROCEDURE sp_ProdutosPorCategoria
    @IdCategoria INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nome, p.Descricao, p.Preco, p.ImagemUrl, p.Calorias, p.Tags, c.Nome AS Categoria
    FROM Produtos p
    LEFT JOIN Categorias c ON c.Id = p.IdCategoria
    WHERE p.Disponivel = 1
      AND (@IdCategoria IS NULL OR p.IdCategoria = @IdCategoria)
    ORDER BY p.Nome;
END
GO

CREATE OR ALTER PROCEDURE sp_AtualizarStatusPedido
    @IdPedido INT,
    @NovoStatus NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Atual NVARCHAR(20);
    SELECT @Atual = Status FROM Pedidos WHERE Id = @IdPedido;

    IF @Atual IS NULL
    BEGIN
        RAISERROR('Pedido nao encontrado.', 16, 1);
        RETURN;
    END

    IF @Atual IN ('Entregue', 'Cancelado')
    BEGIN
        RAISERROR('Pedido ja finalizado.', 16, 1);
        RETURN;
    END

    DECLARE @Ordem TABLE (Status NVARCHAR(20), Posicao INT);
    INSERT INTO @Ordem VALUES ('Recebido', 1), ('Preparando', 2), ('Pronto', 3), ('SaiuParaEntrega', 4), ('Entregue', 5);

    IF @NovoStatus <> 'Cancelado'
    BEGIN
        DECLARE @PosAtual INT = (SELECT Posicao FROM @Ordem WHERE Status = @Atual);
        DECLARE @PosNovo INT = (SELECT Posicao FROM @Ordem WHERE Status = @NovoStatus);

        IF @PosNovo IS NULL OR @PosNovo <> @PosAtual + 1
        BEGIN
            RAISERROR('Transicao de status invalida.', 16, 1);
            RETURN;
        END
    END

    UPDATE Pedidos SET Status = @NovoStatus WHERE Id = @IdPedido;
END
GO

CREATE OR ALTER PROCEDURE sp_RelatorioVendas
    @Inicio DATE,
    @Fim DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CAST(DataHora AS DATE) AS Dia,
           COUNT(*) AS Pedidos,
           SUM(ValorTotal) AS Faturamento,
           AVG(ValorTotal) AS TicketMedio
    FROM Pedidos
    WHERE Status <> 'Cancelado'
      AND CAST(DataHora AS DATE) BETWEEN @Inicio AND @Fim
    GROUP BY CAST(DataHora AS DATE)
    ORDER BY Dia;
END
GO

CREATE OR ALTER PROCEDURE sp_ResumoDashboard
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        (SELECT COUNT(*) FROM Pedidos WHERE CAST(DataHora AS DATE) = CAST(GETDATE() AS DATE)) AS PedidosHoje,
        (SELECT ISNULL(SUM(ValorTotal), 0) FROM Pedidos WHERE Status <> 'Cancelado' AND CAST(DataHora AS DATE) = CAST(GETDATE() AS DATE)) AS FaturamentoHoje,
        (SELECT COUNT(*) FROM Usuarios WHERE Ativo = 1 AND TipoUsuario IN ('cliente', 'empresa')) AS ClientesAtivos,
        (SELECT ISNULL(AVG(ValorTotal), 0) FROM Pedidos WHERE Status <> 'Cancelado') AS TicketMedio,
        (SELECT COUNT(*) FROM Ingredientes WHERE QuantidadeAtual <= QuantidadeMinima) AS IngredientesEmAlerta;
END
GO

CREATE OR ALTER TRIGGER trg_ItensPedido_BaixaEstoque
ON ItensPedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ing
    SET ing.QuantidadeAtual = ing.QuantidadeAtual - uso.Total
    FROM Ingredientes ing
    INNER JOIN (
        SELECT pi.IdIngrediente, SUM(i.Quantidade * pi.QuantidadeNecessaria) AS Total
        FROM inserted i
        INNER JOIN ProdutoIngredientes pi ON pi.IdProduto = i.IdProduto
        GROUP BY pi.IdIngrediente
    ) uso ON uso.IdIngrediente = ing.Id;
END
GO

CREATE OR ALTER TRIGGER trg_Ingredientes_Alerta
ON Ingredientes
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AlertasEstoque (IdIngrediente, Mensagem)
    SELECT i.Id, 'Estoque baixo: ' + i.Nome
    FROM inserted i
    INNER JOIN deleted d ON d.Id = i.Id
    WHERE i.QuantidadeAtual <= i.QuantidadeMinima
      AND d.QuantidadeAtual > d.QuantidadeMinima;
END
GO

CREATE OR ALTER TRIGGER trg_Pedidos_Cancelamento
ON Pedidos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (
        SELECT 1 FROM inserted i INNER JOIN deleted d ON d.Id = i.Id
        WHERE i.Status = 'Cancelado' AND d.Status <> 'Cancelado')
        RETURN;

    UPDATE ing
    SET ing.QuantidadeAtual = ing.QuantidadeAtual + devolvido.Total
    FROM Ingredientes ing
    INNER JOIN (
        SELECT pi.IdIngrediente, SUM(it.Quantidade * pi.QuantidadeNecessaria) AS Total
        FROM inserted i
        INNER JOIN deleted d ON d.Id = i.Id
        INNER JOIN ItensPedido it ON it.IdPedido = i.Id
        INNER JOIN ProdutoIngredientes pi ON pi.IdProduto = it.IdProduto
        WHERE i.Status = 'Cancelado' AND d.Status <> 'Cancelado'
        GROUP BY pi.IdIngrediente
    ) devolvido ON devolvido.IdIngrediente = ing.Id;
END
GO
