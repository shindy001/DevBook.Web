using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DevBook.Web.Client.Features.Account;

public class DevBookAuthenticationStateProvider : AuthenticationStateProvider
{
	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var user = new ClaimsPrincipal();

		return Task.FromResult(new AuthenticationState(user));
	}
}
