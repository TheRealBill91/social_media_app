export async function resetPassword(env: Env, formData: FormData) {
  const passwordResetUserId = String(formData.get("PasswordResetUserId"));
  const code = String(formData.get("Code"));
  const password = String(formData.get("password"));
  const passwordConfirmation = String(formData.get("passwordConfirmation"));

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
