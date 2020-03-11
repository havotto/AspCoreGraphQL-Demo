# ASP.NET Core 3.1 GraphQL demo with Hot Chocolate library
 - Pure code first approach for simple code
 - How to use GroupDataLoader (https://hotchocolate.io/docs/dataloaders#group-dataloader)
 - Filtering, sorting, cursorbased pagination (https://hotchocolate.io/docs/pagination#relay-style-cursor-pagination)
 - Shows a solution for GraphQL executor's DataContext concurrency problem

## How to test
Install .NET Core SDK 3.1: https://dotnet.microsoft.com/download/dotnet-core/3.1

In the project root directory execute this command: dotnet watch run

Open the GraphQL playground at to execute queries: http://localhost:5000/graphql/playground

## Used libraries
 - ASP.NET Core 3.1
 - EF Core 3.1
 - SQLite
 - HotChocolate 10.3.6

## Domain model
Post, Comment, Tag

## Sample queries

### Comments with filter & sort

```
query {
  comments(
    order_by: { id: ASC }, 
    where: { AND: [{ text_starts_with: "Post2" }, {id_lt: 4}] }) {
    id
    text
  }
}
```
