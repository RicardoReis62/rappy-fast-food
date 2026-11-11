IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'RappyFastFoodDB')
BEGIN
    CREATE DATABASE RappyFastFoodDB;
END
GO

USE RappyFastFoodDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Email NVARCHAR(100) NOT NULL UNIQUE,
        SenhaHash NVARCHAR(255) NOT NULL,
        TipoUsuario NVARCHAR(20) NOT NULL
            CHECK (TipoUsuario IN ('cliente', 'empresa', 'admin', 'entregador')),
        Nome NVARCHAR(100) NOT NULL,
        Telefone NVARCHAR(20),
        Ativo BIT DEFAULT 1,
        DataCadastro DATETIME DEFAULT GETDATE()
    );
    CREATE INDEX IX_Usuarios_Tipo ON Usuarios(TipoUsuario);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClientesB2C')
BEGIN
    CREATE TABLE ClientesB2C (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdUsuario INT NOT NULL UNIQUE,
        CPF NVARCHAR(14),
        Endereco NVARCHAR(300),
        Cidade NVARCHAR(50),
        Estado NVARCHAR(2),
        CONSTRAINT FK_ClientesB2C_Usuarios
            FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EmpresasB2B')
BEGIN
    CREATE TABLE EmpresasB2B (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdUsuario INT NOT NULL UNIQUE,
        RazaoSocial NVARCHAR(200),
        CNPJ NVARCHAR(18),
        NomeFantasia NVARCHAR(200),
        QuantidadeFuncionarios INT,
        EnderecoEntrega NVARCHAR(300),
        CONSTRAINT FK_EmpresasB2B_Usuarios
            FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX UX_EmpresasB2B_CNPJ ON EmpresasB2B(CNPJ) WHERE CNPJ IS NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RestaurantesParceiros')
BEGIN
    CREATE TABLE RestaurantesParceiros (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdUsuario INT NOT NULL UNIQUE,
        NomeEstabelecimento NVARCHAR(200) NOT NULL,
        CNPJ NVARCHAR(18),
        Endereco NVARCHAR(300),
        TelefoneComercial NVARCHAR(20),
        Descricao NVARCHAR(500),
        Ativo BIT DEFAULT 1,
        CONSTRAINT FK_RestaurantesParceiros_Usuarios
            FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX UX_RestaurantesParceiros_CNPJ ON RestaurantesParceiros(CNPJ) WHERE CNPJ IS NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categorias')
BEGIN
    CREATE TABLE Categorias (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(50) NOT NULL,
        Descricao NVARCHAR(200),
        TipoPublico NVARCHAR(10)
            CHECK (TipoPublico IN ('B2C', 'B2B', 'Ambos')) DEFAULT 'Ambos',
        Ativo BIT DEFAULT 1
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produtos')
BEGIN
    CREATE TABLE Produtos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(100) NOT NULL,
        Descricao NVARCHAR(500),
        Preco DECIMAL(18,2) NOT NULL,
        CustoProducao DECIMAL(18,2),
        IdCategoria INT,
        ImagemUrl NVARCHAR(500),
        TempoPreparoMinutos INT,
        Disponivel BIT DEFAULT 1,
        EhKitSemanal BIT DEFAULT 0,
        EhParaEvento BIT DEFAULT 0,
        QuantidadeMinimaEvento INT,
        IdRestaurante INT NULL,
        Calorias INT,
        Tags NVARCHAR(200),
        DataCadastro DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Produtos_Categorias
            FOREIGN KEY (IdCategoria) REFERENCES Categorias(Id),
        CONSTRAINT FK_Produtos_Restaurantes
            FOREIGN KEY (IdRestaurante) REFERENCES RestaurantesParceiros(Id) ON DELETE SET NULL
    );
    CREATE INDEX IX_Produtos_Categoria ON Produtos(IdCategoria);
    CREATE INDEX IX_Produtos_Disponivel ON Produtos(Disponivel);
    CREATE INDEX IX_Produtos_Restaurante ON Produtos(IdRestaurante);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ingredientes')
BEGIN
    CREATE TABLE Ingredientes (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(100) NOT NULL,
        UnidadeMedida NVARCHAR(20),
        QuantidadeAtual DECIMAL(18,2) DEFAULT 0,
        QuantidadeMinima DECIMAL(18,2) DEFAULT 0,
        PrecoUnitario DECIMAL(18,2),
        IdRestaurante INT,
        CONSTRAINT FK_Ingredientes_Restaurantes
            FOREIGN KEY (IdRestaurante) REFERENCES RestaurantesParceiros(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_Ingredientes_Restaurante ON Ingredientes(IdRestaurante);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProdutoIngredientes')
BEGIN
    CREATE TABLE ProdutoIngredientes (
        IdProduto INT NOT NULL,
        IdIngrediente INT NOT NULL,
        QuantidadeNecessaria DECIMAL(18,2) NOT NULL,
        PRIMARY KEY (IdProduto, IdIngrediente),
        CONSTRAINT FK_PI_Produtos
            FOREIGN KEY (IdProduto) REFERENCES Produtos(Id) ON DELETE CASCADE,
        CONSTRAINT FK_PI_Ingredientes
            FOREIGN KEY (IdIngrediente) REFERENCES Ingredientes(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedidos')
BEGIN
    CREATE TABLE Pedidos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        TipoCliente NVARCHAR(3) NOT NULL
            CHECK (TipoCliente IN ('B2C', 'B2B')),
        IdClienteB2C INT NULL,
        IdEmpresaB2B INT NULL,
        DataHora DATETIME DEFAULT GETDATE(),
        TipoEntrega NVARCHAR(20) DEFAULT 'Delivery',
        Status NVARCHAR(20) DEFAULT 'Recebido'
            CHECK (Status IN ('Recebido', 'Preparando', 'Pronto', 'SaiuParaEntrega', 'Entregue', 'Cancelado')),
        EnderecoEntrega NVARCHAR(300),
        ValorTotal DECIMAL(18,2) NOT NULL,
        FormaPagamento NVARCHAR(20)
            CHECK (FormaPagamento IN ('PIX', 'CartaoCredito', 'CartaoDebito', 'Dinheiro')),
        Observacoes NVARCHAR(500),
        TaxaEntrega DECIMAL(18,2) DEFAULT 0,
        Desconto DECIMAL(18,2) DEFAULT 0,
        CONSTRAINT FK_Pedidos_ClientesB2C
            FOREIGN KEY (IdClienteB2C) REFERENCES ClientesB2C(Id),
        CONSTRAINT FK_Pedidos_EmpresasB2B
            FOREIGN KEY (IdEmpresaB2B) REFERENCES EmpresasB2B(Id),
        CONSTRAINT CK_Pedidos_Cliente CHECK (
            (TipoCliente = 'B2C' AND IdClienteB2C IS NOT NULL AND IdEmpresaB2B IS NULL)
            OR (TipoCliente = 'B2B' AND IdEmpresaB2B IS NOT NULL AND IdClienteB2C IS NULL))
    );
    CREATE INDEX IX_Pedidos_Status ON Pedidos(Status);
    CREATE INDEX IX_Pedidos_DataHora ON Pedidos(DataHora DESC);
    CREATE INDEX IX_Pedidos_Cliente ON Pedidos(IdClienteB2C);
    CREATE INDEX IX_Pedidos_Empresa ON Pedidos(IdEmpresaB2B);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ItensPedido')
BEGIN
    CREATE TABLE ItensPedido (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdPedido INT NOT NULL,
        IdProduto INT NOT NULL,
        Quantidade INT NOT NULL CHECK (Quantidade > 0),
        ValorUnitario DECIMAL(18,2) NOT NULL,
        Observacao NVARCHAR(200),
        CONSTRAINT FK_ItensPedido_Pedidos
            FOREIGN KEY (IdPedido) REFERENCES Pedidos(Id) ON DELETE CASCADE,
        CONSTRAINT FK_ItensPedido_Produtos
            FOREIGN KEY (IdProduto) REFERENCES Produtos(Id)
    );
    CREATE INDEX IX_ItensPedido_Pedido ON ItensPedido(IdPedido);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Avaliacoes')
BEGIN
    CREATE TABLE Avaliacoes (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdPedido INT NOT NULL,
        Nota INT NOT NULL CHECK (Nota BETWEEN 1 AND 5),
        Comentario NVARCHAR(500),
        DataAvaliacao DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Avaliacoes_Pedidos
            FOREIGN KEY (IdPedido) REFERENCES Pedidos(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Favoritos')
BEGIN
    CREATE TABLE Favoritos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdUsuario INT NOT NULL,
        IdProduto INT NOT NULL,
        DataAdicionado DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Favoritos_Usuarios
            FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Favoritos_Produtos
            FOREIGN KEY (IdProduto) REFERENCES Produtos(Id) ON DELETE CASCADE,
        CONSTRAINT UQ_Favoritos_UsuarioProduto UNIQUE (IdUsuario, IdProduto)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AlertasEstoque')
BEGIN
    CREATE TABLE AlertasEstoque (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IdIngrediente INT NOT NULL,
        Mensagem NVARCHAR(200) NOT NULL,
        DataHora DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_AlertasEstoque_Ingredientes
            FOREIGN KEY (IdIngrediente) REFERENCES Ingredientes(Id) ON DELETE CASCADE
    );
END
GO
