# AspCoreGraphQL-Demo

Shows a solution for GraphQL executor's DataContext concurrency problem.

http://localhost:5000/graphql/playground

## Tools
 - ASP.NET Core 3.1
 - EF Core 3.1
 - SQLite
 - HotChocolate

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
