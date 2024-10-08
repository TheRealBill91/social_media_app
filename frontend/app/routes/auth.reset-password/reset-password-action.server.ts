export async function resetPassword(
  env: Env,
  passwordResetUserId: string,
  code: string,
  password: string,
  passwordConfirmation: string,
) {
  const resetPasswordResponse = await fetch(
    `${env.API_URL}/api/auth/reset-password`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        newPassword: password,
        newPasswordConfirmation: passwordConfirmation,
        code,
        passwordResetUserId,
      }),
    },
  );

  return resetPasswordResponse;
}
