/**
 * fetch request to the dotnet api for logging out
 * @param authCookie
 * @param env
 * @returns
 */

export async function logout(authCookie: string, env: Env) {
  const logoutResult = await fetch(`${env.API_URL}/api/auth/signout`, {
    method: "POST",
    headers: {
      cookie: authCookie,
    },
  });

  return logoutResult;
}
