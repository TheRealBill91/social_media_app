export async function logout(authCookie: string, env: Env) {
  const logoutResult = await fetch(`${env.API_URL}/api/auth/signout`, {
    method: "POST",
    headers: {
      cookie: authCookie,
    },
  });

  return logoutResult;
}
