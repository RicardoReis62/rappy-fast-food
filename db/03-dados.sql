USE RappyFastFoodDB;
GO

IF NOT EXISTS (SELECT 1 FROM Categorias)
BEGIN
    INSERT INTO Categorias (Nome, Descricao, TipoPublico) VALUES
    ('Low Carb', 'Refeicoes com baixo teor de carboidrato', 'Ambos'),
    ('Vegano', 'Refeicoes sem ingredientes de origem animal', 'Ambos'),
    ('Sem Gluten', 'Refeicoes livres de gluten', 'Ambos'),
    ('Executivo', 'Marmitas e kits para empresas', 'B2B'),
    ('Bebidas', 'Sucos e aguas', 'Ambos');
END
GO

IF NOT EXISTS (SELECT 1 FROM Ingredientes)
BEGIN
    INSERT INTO Ingredientes (Nome, UnidadeMedida, QuantidadeAtual, QuantidadeMinima, PrecoUnitario) VALUES
    ('Frango Grelhado', 'kg', 180, 12.5, 22.00),
    ('Quinoa', 'kg', 120, 5, 18.00),
    ('Arroz', 'kg', 120, 12.5, 6.00),
    ('Feijao', 'kg', 240, 7.5, 8.00),
    ('Carne Bovina', 'kg', 180, 6.25, 38.00),
    ('Salmao', 'kg', 80, 5, 65.00),
    ('Brocolis', 'kg', 150, 7.5, 9.00),
    ('Alface', 'un', 250, 25, 3.00),
    ('Tomate', 'kg', 250, 10, 7.00),
    ('Grao de Bico', 'kg', 90, 6, 12.00),
    ('Abobrinha', 'kg', 150, 6.25, 6.50),
    ('Laranja', 'kg', 200, 10, 5.00),
    ('Couve', 'kg', 90, 12, 5.50);
END
GO

IF NOT EXISTS (SELECT 1 FROM Produtos)
BEGIN
    INSERT INTO Produtos (Nome, Descricao, Preco, CustoProducao, IdCategoria, TempoPreparoMinutos, Calorias, Tags) VALUES
    ('Bowl de Quinoa Low Carb', 'Quinoa com frango grelhado e brocolis', 32.90, 14.00, 1, 20, 420, 'low carb,proteina'),
    ('Salmao Grelhado', 'Salmao com legumes salteados', 44.90, 22.00, 1, 25, 480, 'low carb,omega3'),
    ('Frango com Legumes', 'Peito de frango grelhado com abobrinha', 29.90, 12.00, 1, 20, 380, 'low carb'),
    ('Bowl Vegano de Grao de Bico', 'Grao de bico, quinoa e legumes', 31.90, 11.00, 2, 20, 400, 'vegano'),
    ('Curry de Legumes', 'Curry cremoso de legumes com arroz', 28.90, 9.50, 2, 25, 450, 'vegano'),
    ('Carne com Arroz e Feijao', 'Carne bovina, arroz, feijao e salada', 34.90, 15.00, 3, 20, 620, 'sem gluten'),
    ('Combo Executivo', 'Marmita completa para equipes', 27.90, 11.50, 4, 30, 650, 'executivo'),
    ('Kit Semanal 5 Marmitas', 'Cinco refeicoes variadas', 139.90, 60.00, 4, 40, 2800, 'kit,semanal'),
    ('Suco Verde Detox', 'Couve, laranja e limao', 12.90, 4.00, 5, 5, 90, 'bebida'),
    ('Agua de Coco', 'Agua de coco natural 500ml', 9.90, 4.50, 5, 2, 60, 'bebida');

    INSERT INTO ProdutoIngredientes (IdProduto, IdIngrediente, QuantidadeNecessaria) VALUES
    (1, 2, 0.15), (1, 1, 0.15), (1, 7, 0.10),
    (2, 6, 0.20), (2, 11, 0.10),
    (3, 1, 0.20), (3, 11, 0.15),
    (4, 10, 0.15), (4, 2, 0.10), (4, 9, 0.05),
    (5, 11, 0.15), (5, 3, 0.15),
    (6, 5, 0.18), (6, 3, 0.15), (6, 4, 0.10), (6, 8, 0.50),
    (7, 1, 0.15), (7, 3, 0.15), (7, 4, 0.10),
    (9, 13, 0.10), (9, 12, 0.20);
END
GO
