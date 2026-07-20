interface Props {
  message: string;
  onRetry?: () => void;
}

export default function ErrorMessage({ message, onRetry }: Props) {
  return (
    <div className="flex flex-col items-center justify-center py-16 gap-3">
      <div className="text-red-400 text-4xl">⚠</div>
      <p className="text-gray-700 font-medium">{message}</p>
      {onRetry && (
        <button
          onClick={onRetry}
          className="px-4 py-2 text-sm bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          Try again
        </button>
      )}
    </div>
  );
}
