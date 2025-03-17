# Resources

Good walkthrough of signalr chatapp in .NET MVC
https://www.youtube.com/watch?v=_gJ0NbNKKZg

Guide on creating a backend api in asp.net with react front end
https://www.youtube.com/watch?v=pvi_ZS_PrSc

https://dev.to/javier1984/how-to-build-a-performant-signalr-chat-spa-using-angular-10-and-net-core-3-1-4ghn
https://stackoverflow.com/questions/63342818/signalr-with-angular-10-and-asp-net

official doc walkthrough
https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr

## password hashing

https://dotnettutorials.net/lesson/how-to-store-password-in-hash-format-in-asp-net-core-web-api/

https://medium.com/@nambi2210/password-hashing-in-asp-net-core-ee377c29fa24

> Have copied the hashing and verifying directly from official asp.net core codebase
> TODO: See how the default implementation with ASP.NET Identity works with mongodb and make relevant changes
> https://github.dev/dotnet/AspNetCore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs

new hashing changes
https://www.youtube.com/watch?v=J4ix8Mhi3rs

understanding secrets in ASP.NET to store database strings and jwt secrets
https://www.youtube.com/watch?v=PkLLP2tcd28

Understanding options and setup in asp.net
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0
https://www.youtube.com/watch?v=SizJCLcjbOA

Setting identity for signalr users
https://stackoverflow.com/questions/48110615/userid-in-signalr-core

Github Setup for .net web api
https://khalidabuhakmeh.com/github-openid-auth-aspnet-core-apps
https://www.youtube.com/watch?v=PUXpfr1LzPE

Initial flow was to have the callback in API, but decided to have the flow through Frontend, based on suggestion below. Flows better.
https://stackoverflow.com/questions/75473126/oauth2-method-for-callback-url-from-backend-to-frontend

Github callback response

```
{
  "login": "nusrat-ullah",
  "id": 187502460,
  "node_id": "U_kgDOCy0PfA",
  "avatar_url": "https://avatars.githubusercontent.com/u/187502460?v=4",
  "gravatar_id": "",
  "url": "https://api.github.com/users/nusrat-ullah",
  "html_url": "https://github.com/nusrat-ullah",
  "followers_url": "https://api.github.com/users/nusrat-ullah/followers",
  "following_url": "https://api.github.com/users/nusrat-ullah/following{/other_user}",
  "gists_url": "https://api.github.com/users/nusrat-ullah/gists{/gist_id}",
  "starred_url": "https://api.github.com/users/nusrat-ullah/starred{/owner}{/repo}",
  "subscriptions_url": "https://api.github.com/users/nusrat-ullah/subscriptions",
  "organizations_url": "https://api.github.com/users/nusrat-ullah/orgs",
  "repos_url": "https://api.github.com/users/nusrat-ullah/repos",
  "events_url": "https://api.github.com/users/nusrat-ullah/events{/privacy}",
  "received_events_url": "https://api.github.com/users/nusrat-ullah/received_events",
  "type": "User",
  "user_view_type": "private",
  "site_admin": false,
  "name": null,
  "company": null,
  "blog": "",
  "location": null,
  "email": null,
  "hireable": null,
  "bio": null,
  "twitter_username": null,
  "notification_email": null,
  "public_repos": 2,
  "public_gists": 0,
  "followers": 0,
  "following": 0,
  "created_at": "2024-11-06T09:47:08Z",
  "updated_at": "2024-11-17T20:19:29Z",
  "private_gists": 0,
  "total_private_repos": 0,
  "owned_private_repos": 0,
  "disk_usage": 6,
  "collaborators": 0,
  "two_factor_authentication": false,
  "plan": {
    "name": "free",
    "space": 976562499,
    "collaborators": 0,
    "private_repos": 10000
  }
}
```

TODO: Decide what to do if the email is not present in github response. Make it non-mandatory?


# .NET Dockerize
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-9.0
https://medium.com/@marcus121neo/asp-net-core-multi-environment-setup-with-docker-7591a872bacc
https://stackoverflow.com/a/69738798
