export function AuthDivider() {
  return (
    <div className="relative my-8 w-full">
      <div className="absolute inset-0 flex w-full items-center">
        <div className="w-full border-t border-gray-400/80"></div>
      </div>
      <div className="relative flex justify-center text-sm capitalize">
        <p className="bg-white px-3 uppercase text-gray-600">
          or continue with
        </p>
      </div>
    </div>
  );
}
