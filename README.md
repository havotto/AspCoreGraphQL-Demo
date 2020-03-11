# AspCoreGraphQL-Demo

Shows a solution for GraphQL executor's DataContext concurrency problem.

## Running

dotnet watch run

Playground url: http://localhost:5000/graphql/playground

## Tools
 - .NET Core 3.1 SDK
 - ASP.NET Core 3.1
 - EF Core 3.1
 - SQLite
 - HotChocolate 10.3.6

## Domain
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
