CREATE DATABASE ACADEMIACORPOALMA;
GO

USE ACADEMIACORPOALMA;
GO


CREATE TABLE Pessoa (
    id_pessoa INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);
GO


CREATE TABLE Aluno (
    id_aluno INT PRIMARY KEY,
    telefone VARCHAR(20) NOT NULL,
    status_aluno VARCHAR(20) NOT NULL,

    FOREIGN KEY (id_aluno)
        REFERENCES Pessoa(id_pessoa)
);
GO


CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY,
    email VARCHAR(120) NOT NULL UNIQUE,
    senha VARCHAR(100) NOT NULL,
    perfil VARCHAR(20) NOT NULL,

    FOREIGN KEY (id_usuario)
        REFERENCES Pessoa(id_pessoa)
);
GO


CREATE TABLE Plano (
    id_plano INT IDENTITY(1,1) PRIMARY KEY,
    nome_plano VARCHAR(50) NOT NULL,
    valor_plano DECIMAL(10,2) NOT NULL
);
GO


CREATE TABLE Matricula (
    id_matricula INT IDENTITY(1,1) PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_plano INT NOT NULL,
    id_usuario INT NOT NULL,
    data_matricula DATE NOT NULL,
    dia_pagamento INT NOT NULL,
    ultimo_pagamento DATE NULL,
    meses_em_atraso INT NOT NULL,

    FOREIGN KEY (id_aluno)
        REFERENCES Aluno(id_aluno),

    FOREIGN KEY (id_plano)
        REFERENCES Plano(id_plano),

    FOREIGN KEY (id_usuario)
        REFERENCES Usuario(id_usuario)
);
GO


CREATE TABLE Pagamento (
    id_pagamento INT IDENTITY(1,1) PRIMARY KEY,
    id_matricula INT NOT NULL,
    data_pagamento DATE NOT NULL,
    forma_pagamento VARCHAR(20) NOT NULL,

    FOREIGN KEY (id_matricula)
        REFERENCES Matricula(id_matricula)
);
GO


CREATE TABLE Ficha_Treino (
    id_treino INT IDENTITY(1,1) PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_usuario INT NOT NULL,
    nome_treino VARCHAR(100) NOT NULL,
    data_criacao DATE NOT NULL,
    objetivo VARCHAR(150) NOT NULL,

    FOREIGN KEY (id_aluno)
        REFERENCES Aluno(id_aluno),

    FOREIGN KEY (id_usuario)
        REFERENCES Usuario(id_usuario)
);
GO


CREATE TABLE Exercicio (
    id_exercicio INT IDENTITY(1,1) PRIMARY KEY,
    nome_exercicio VARCHAR(100) NOT NULL,
    grupo_muscular VARCHAR(100) NOT NULL,
    series INT NOT NULL,
    repeticoes INT NOT NULL,
    carga_kg DECIMAL(6,2) NOT NULL
);
GO


CREATE TABLE Ficha_Exercicio (
    id_treino INT NOT NULL,
    id_exercicio INT NOT NULL,

    PRIMARY KEY (id_treino, id_exercicio),

    FOREIGN KEY (id_treino)
        REFERENCES Ficha_Treino(id_treino),

    FOREIGN KEY (id_exercicio)
        REFERENCES Exercicio(id_exercicio)
);
GO


INSERT INTO Pessoa (nome) VALUES ('João Silva');
INSERT INTO Pessoa (nome) VALUES ('Maria Souza');
INSERT INTO Pessoa (nome) VALUES ('Pedro Santos');
INSERT INTO Pessoa (nome) VALUES ('Maria Eduarda Nogueira');
INSERT INTO Pessoa (nome) VALUES ('Wilian');
INSERT INTO Pessoa (nome) VALUES ('Rodrigo Oliveira');
GO


INSERT INTO Aluno (id_aluno, telefone, status_aluno) VALUES (1, '(17) 99999-1101', 'ATIVO');
INSERT INTO Aluno (id_aluno, telefone, status_aluno) VALUES (2, '(17) 98888-2244', 'ATIVO');
INSERT INTO Aluno (id_aluno, telefone, status_aluno) VALUES (3, '(17) 97777-3322', 'ATIVO');
GO


INSERT INTO Usuario (id_usuario, email, senha, perfil) VALUES (4, 'maria.eduarda@corpoealma.com', '123456', 'FUNCIONARIO');
INSERT INTO Usuario (id_usuario, email, senha, perfil) VALUES (5, 'wilian@corpoealma.com', '123456', 'FUNCIONARIO');
INSERT INTO Usuario (id_usuario, email, senha, perfil) VALUES (6, 'rodrigo@corpoealma.com', '123456', 'DONO');
GO


INSERT INTO Plano (nome_plano, valor_plano) VALUES ('Todos os dias', 75.00);
INSERT INTO Plano (nome_plano, valor_plano) VALUES ('3 vezes por semana', 70.00);
GO


INSERT INTO Matricula (id_aluno, id_plano, id_usuario, data_matricula, dia_pagamento, ultimo_pagamento, meses_em_atraso) VALUES (1, 1, 4, '2026-03-10', 10, '2026-10-10', 0);
INSERT INTO Matricula (id_aluno, id_plano, id_usuario, data_matricula, dia_pagamento, ultimo_pagamento, meses_em_atraso) VALUES (2, 2, 5, '2026-04-21', 21, '2026-09-21', 1);
INSERT INTO Matricula (id_aluno, id_plano, id_usuario, data_matricula, dia_pagamento, ultimo_pagamento, meses_em_atraso) VALUES (3, 1, 4, '2026-05-15', 15, '2026-10-15', 0);
GO


INSERT INTO Pagamento (id_matricula, data_pagamento, forma_pagamento) VALUES (1, '2026-10-10', 'PIX');
INSERT INTO Pagamento (id_matricula, data_pagamento, forma_pagamento) VALUES (2, '2026-09-21', 'DINHEIRO');
INSERT INTO Pagamento (id_matricula, data_pagamento, forma_pagamento) VALUES (3, '2026-10-15', 'PIX');
GO


INSERT INTO Ficha_Treino (id_aluno, id_usuario, nome_treino, data_criacao, objetivo) VALUES (1, 4, 'Treino A', '2026-10-01', 'Hipertrofia');
INSERT INTO Ficha_Treino (id_aluno, id_usuario, nome_treino, data_criacao, objetivo) VALUES (2, 5, 'Treino B', '2026-10-02', 'Condicionamento físico');
INSERT INTO Ficha_Treino (id_aluno, id_usuario, nome_treino, data_criacao, objetivo) VALUES (3, 4, 'Treino C', '2026-10-03', 'Ganho de massa muscular');
GO


INSERT INTO Exercicio (nome_exercicio, grupo_muscular, series, repeticoes, carga_kg) VALUES ('Supino Reto', 'Peitoral', 3, 10, 20.00);
INSERT INTO Exercicio (nome_exercicio, grupo_muscular, series, repeticoes, carga_kg) VALUES ('Agachamento', 'Pernas', 4, 12, 30.00);
INSERT INTO Exercicio (nome_exercicio, grupo_muscular, series, repeticoes, carga_kg) VALUES ('Rosca Direta', 'Bíceps', 3, 12, 10.00);
INSERT INTO Exercicio (nome_exercicio, grupo_muscular, series, repeticoes, carga_kg) VALUES ('Puxada Frontal', 'Costas', 3, 10, 25.00);
GO


INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (1, 1);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (1, 3);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (2, 2);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (2, 4);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (3, 1);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (3, 2);
INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (3, 3);
GO