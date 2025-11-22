![Banner](./docs/banner.png)

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)]()
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)]()
[![FIAP](https://img.shields.io/badge/FIAP-ED145B?style=for-the-badge&logoColor=white)]()

O Chameleon Future Academy é uma plataforma educacional criada para ajudar pessoas cujas profissões estão sendo impactadas ou substituídas pelos avanços tecnológicos. Seu objetivo é facilitar a migração de carreira, oferecendo cursos focados em habilidades relevantes para o mercado do futuro.

A aplicação permite criar e gerenciar contas de usuário, consultar cursos, acessar aulas e atividades, realizar matrículas e conquistar badges ao concluir formações. Tudo isso compondo um perfil acadêmico dinâmico e em constante evolução.

> Este repositório contém os arquivos da API Administrativa do Chameleon Future Academy, desenvolvida com .NET.

---

[Arquitetura](#arquitetura) | [Endpoints](#endpoints) | [Setup do Projeto](#setup-do-projeto) | [Stack Tecnológica](#stack-tecnológica) | [Desenvolvedores](#desenvolvedores)

---

## Arquitetura

A solução foi estruturada em uma arquitetura modular e separada por responsabilidades, garantindo organização, escalabilidade e facilidade de manutenção.

- **API Front Office (Spring Boot):** Responsável por atender o app mobile do usuário final. Foi separada para garantir alto desempenho, rotas enxutas e um backend otimizado exclusivamente para a experiência do cliente.
- **API Back Office (.NET):** Usada apenas pelo painel administrativo. Mantida isolada para reforçar segurança, permitir controles mais sensíveis (gestão de cursos, conteúdos e etc) e evitar que lógicas administrativas impactem o fluxo do usuário final.
- **Banco de Dados Oracle:** Centraliza e integra os dados das duas APIs. Escolhido pela robustez e pela confiabilidade em ambientes corporativos, garantindo consistência transacional e escalabilidade.
- **Bucket para mídias:** Armazenamento externo para imagens e arquivos. Evita tráfego desnecessário nas APIs, reduz carga no banco e facilita a distribuição de conteúdo estático

### Estrutura de Pastas .NET

- **Data:** Configuração do contexto de banco (AppDbContext)  
- **DTOs:** Objetos de transferência usados para organizar dados de entrada/saída nas rotas  
- **Endpoints:** Implementação dos endpoints REST, seguindo o padrão de Minimal APIs  
- **Filters:** Filtros de requisição, incluindo validações de dados com tratamento padronizado de erros  
- **Hateoas:** Implementações de navegação entre recursos (links, resources)  
- **Models:** Entidades do domínio representando a estrutura principal dos dados  
- **Services:** Serviços auxiliares usados pelas rotas

## Endpoints

Os endpoints da API de back office servem para que administradores gerenciem cursos e suas dependências (como aulas, atividades e badges).

A seguir estão listados os principais endpoints disponíveis na API Back Office (Este repositório).

### Courses

```
GET    /courses        Lista todos os cursos  
GET    /courses/search Busca cursos por título  
GET    /courses/{id}   Retorna um curso pelo ID  
POST   /courses        Cria um novo curso  
PUT    /courses/{id}   Atualiza um curso existente  
DELETE /courses/{id}   Remove um curso do sistema  
```

### Tags

```
GET    /tags        Lista todas as tags  
GET    /tags/search Busca tags por descrição  
GET    /tags/{id}   Retorna uma tag pelo ID  
POST   /tags        Cria uma nova tag  
PUT    /tags/{id}   Atualiza uma tag existente  
DELETE /tags/{id}   Remove uma tag do sistema  
```

### Contents

```
GET    /contents        Lista todos os conteúdos  
GET    /contents/search Busca conteúdos pelo ID do curso ao qual pertence  
GET    /contents/{id}   Retorna um conteúdo pelo ID  
POST   /contents        Cria um novo conteúdo  
PUT    /contents/{id}   Atualiza um conteúdo existente  
DELETE /contents/{id}   Remove um conteúdo do sistema  
```

### Lessons

```
GET    /lesson        Lista todas as lições  
GET    /lesson/search Busca lições pelo ID do conteúdo ao qual pertence  
GET    /lesson/{id}   Retorna uma lição pelo ID  
POST   /lesson        Cria uma nova lição  
PUT    /lesson/{id}   Atualiza uma lição existente  
DELETE /lesson/{id}   Remove uma lição do sistema   
```

### Atividades

```
GET    /activities        Lista todas as atividades  
GET    /activities/search Busca atividades pelo ID do conteúdo ao qual pertence  
GET    /activities/{id}   Retorna uma atividade pelo ID  
POST   /activities        Cria uma nova atividade  
PUT    /activities/{id}   Atualiza uma atividade existente  
DELETE /activities/{id}   Remove uma atividade do sistema  
```

### Alternativas de Atividades

```
GET    /activity-options        Lista todas as alternativas de atividade  
GET    /activity-options/search Busca alternativas de atividade pelo ID da atividade ao qual pertence  
GET    /activity-options/{id}   Retorna uma alternativa de atividade pelo ID  
POST   /activity-options        Cria uma nova alternativa de atividade  
PUT    /activity-options/{id}   Atualiza uma alternativa de atividade existente  
DELETE /activity-options/{id}   Remove uma alternativa de atividade do sistema  
```

### Badges

```
GET    /badges        Lista todos os badges  
GET    /badges/search Busca badges pelo ID do curso ao qual pertence  
GET    /badges/{id}   Retorna um badge pelo ID  
POST   /badges        Cria um novo badge  
PUT    /badges/{id}   Atualiza um badge existente  
DELETE /badges/{id}   Remove um badge do sistema  
```

### Paginação

Alguns endpoints GET suportam paginação para controlar a quantidade de resultados retornados. Os parâmetros são enviados como query params.

Exemplo real:
```
GET /courses?page=1&size=10
```

### Busca

Alguns endpoints GET permitem realizar buscas filtradas, enviando parâmetros específicos na URL. Esses valores também são enviados como query params.

Exemplo real:
```
GET /courses/search?title=Iniciante
```

### Exemplos de Uso

Criação de um novo curso
```
POST /courses
Content-Type: application/json

{
    "title": "Lógica de Programação",
    "description": "Curso introdutório para iniciantes",
    "author": "Equipe Chameleon"
}
```

**Exemplo de reposta**
```
{
    "course_id": 3,
    "title": "Lógica de Programação",
    "description": "Curso introdutório para iniciantes",
    "author": "Equipe Chameleon",
    "thumbnail_url": null,
    "createdAt": "2025-01-21T19:22:15Z"
}
```

Consulta de um curso específico
```
GET /courses/3
```

**Exemplo de reposta**
```
{
    "course_id": 3,
    "title": "Lógica de Programação",
    "description": "Curso introdutório para iniciantes",
    "author": "Equipe Chameleon",
    "thumbnail_url": null,
    "createdAt": "2025-01-21T19:22:15Z"
}
```

Exclusão de um curso
```
DELETE /courses/3
```

**Exemplo de reposta**
```
204 No Content
```

## Setup do Projeto

### Instalação Local

- **Git**
- **.NET SDK** (versão 9.0 ou superior)

#### 1. Clonar Repositório
```bash
# Clonar o repositório
git clone https://github.com/PauloSergioFB/chameleon-future-academy-admin-api.git

# Acessar o diretório
cd chameleon-future-academy-admin-api

# Instalar as dependências
dotnet restore
```

#### 2. Configurar o Ambiente

Crie um arquivo .env na raiz do projeto com o seguinte conteúdo (substitua pelas suas próprias credenciais e configurações):

```bash
ConnectionStrings__OracleConnection=Data Source=oracle.fiap.com.br:1521/orcl;User Id=<seu_usuario>;Password=<sua_senha>;
ASPNETCORE_ENVIRONMENT=Development
```

#### 3. Iniciar o projeto

```bash
dotnet run
```

Após a inicialização, a API estará disponível em: http://localhost:5070  
A documentação interativa (Swagger UI) pode ser acessada em: http://localhost:5070/scalar

### Execução Via Docker

Antes de iniciar, certifique-se de ter instalado:

- **Git**
- **Docker**

#### 1. Clonar Repositório
```bash
# Clonar o repositório
git clone https://github.com/PauloSergioFB/chameleon-future-academy-admin-api.git

# Acessar o diretório
cd chameleon-future-academy-admin-api
```

#### 2. Configurar o Ambiente

Crie um arquivo .env na raiz do projeto com o seguinte conteúdo (substitua pelas suas próprias credenciais e configurações):

```bash
ConnectionStrings__OracleConnection=Data Source=oracle.fiap.com.br:1521/orcl;User Id=<seu_usuario>;Password=<sua_senha>;
ASPNETCORE_ENVIRONMENT=Development
```

#### 3. Iniciar o projeto

```bash
# Construir a imagem do projeto
docker build -t chameleon-future-academy-admin-api .

# Executar o container (carregando as variáveis de .env)
docker run -it --rm --env-file .env -p 5070:5070 chameleon-future-academy-admin-api
```

Após a inicialização, a API estará disponível em: http://localhost:5070  
A documentação interativa (Swagger UI) pode ser acessada em: http://localhost:5070/scalar

## Stack Tecnológica

O projeto utiliza as seguintes tecnologias:

- C# 12 - Linguagem principal utilizada na API.
- .NET 9 - Framework base para construção da aplicação com alto desempenho e suporte multiplataforma.
- Entity Framework Core - ORM utilizado para persistência e mapeamento objeto-relacional.
- Data Annotations - Mapeamento de entidades e validação de dados através de atributos.
- Minimal API - Abordagem leve e direta para definição dos endpoints HTTP.
- Swagger / OpenAPI - Ferramenta para documentação e teste interativo dos endpoints da API.

## Desenvolvedores

[@AntonioDeLuca](https://github.com/antoniodeluca) - Desenvolvedor Backend  
[@EnzoAzevedo](https://github.com/enzoazevedo) - Desenvolvedor Backend  
[@PauloSérgioFB](https://github.com/paulgramador) - Desenvolvedor Mobile
