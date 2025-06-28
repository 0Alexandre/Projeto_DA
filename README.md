![Logo IPL](https://inforestudante.ipleiria.pt/nonio/util/obtemConteudoFicheiroImagemDadosLayoutInstituicao.do?codigo=IMAGEM_HEADER_NORMAL_INFORESTUDANTE&v=1749831571028)

# 📋 iTasks – Gestão de Tarefas Kanban

O **iTasks** é uma aplicação desktop desenvolvida em **C# (.NET Framework)** destinada à gestão interna de tarefas, baseada no conceito Kanban. Oferece funcionalidades completas de gestão para Gestores e Programadores.

---

## 🛠️ Funcionalidades Principais

### 🔐 Login & Sessão

* Criação inicial automática de conta para Gestor, caso a base de dados esteja vazia.
* Autenticação simplificada por Username e Password.

### 👥 Gestão de Utilizadores (exclusivo Gestores)

* CRUD completo de Gestores e Programadores.
* Atribuição de Programadores aos respetivos Gestores.
* Definição de departamentos e níveis de experiência.

### 📑 Gestão de Tipos de Tarefa (exclusivo Gestores)

* CRUD de categorias (ex.: Bug, Feature).

### 🗂️ Kanban

* Listas dinâmicas: **ToDo**, **Doing** e **Done**.
* Criação de tarefas (exclusivo Gestores), incluindo descrição, tipo, programador responsável, ordem, pontos de história e datas previstas.
* Movimentação de tarefas (exclusivo Programadores nas suas próprias tarefas):

  * Limite máximo de 2 tarefas em andamento (Doing).
  * Avanço ou conclusão somente na ordem definida.
* Remoção de tarefas na lista ToDo (exclusivo Gestores).

### 📤 Exportação CSV (exclusivo Gestores)

* Exportação das tarefas concluídas em formato CSV com as seguintes colunas:

  * `Programador;Descricao;DataPrevistaInicio;DataPrevistaFim;TipoTarefa;DataRealInicio;DataRealFim`

---

## 📂 Estrutura do Repositório

```
iTasks
├── Controllers/        # Lógica de negócio (Login, Gestor, Programador, Tarefa, TipoTarefa)
├── Model/              # Entidades (Utilizador, Gestor, Programador, Tarefa, TipoTarefa)
├── View/               # Forms WinForms (Login, Kanban, Gestão, Detalhes)
├── App.config          # Configuração da ligação à base de dados
├── Database.mdf        # Base de dados LocalDB (Entity Framework)
├── Projeto.sln         # Solution (.sln) e projetos (.csproj)
└── README.md           # Documentação do projeto
```

---

## 🚀 Tecnologias e Dependências

* Plataforma: **.NET Framework**
* Interface: **Windows Forms**
* ORM: **Entity Framework**
* Base de Dados: **SQL Server Express LocalDB**
* Controlo de Versões: **Git / GitHub**

---

* ## Autores

* Alexandre Simões
* Guilherme Fernandes
* Frederico Gonçalves

