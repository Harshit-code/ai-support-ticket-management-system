interface Props {
  title: string;
  subtitle?: string;
}

export default function EmptyState({ title, subtitle }: Props) {
  return (
    <div className="flex flex-col items-center justify-center py-16 gap-2 text-gray-400">
      <div className="text-5xl">📭</div>
      <p className="font-medium text-gray-600">{title}</p>
      {subtitle && <p className="text-sm">{subtitle}</p>}
    </div>
  );
}
