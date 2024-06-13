import { cn } from "~/utils/misc";
import { NavLink } from "@remix-run/react";
import { buttonVariants } from "~/components/ui/Button";

interface SidebarNavProps extends React.HTMLAttributes<HTMLElement> {
  items: {
    title: string;
    href: string;
  }[];
}

export function SidebarNav({ className, items, ...props }: SidebarNavProps) {
  return (
    <nav className={cn("flex lg:flex-col", className)} {...props}>
      {items.map((item) => (
        <NavLink
          key={item.href}
          className={({ isActive }) => {
            return cn(
              buttonVariants({ variant: "ghost" }),
              isActive
                ? "bg-muted text-left hover:bg-muted"
                : "text-left hover:bg-transparent",
              "justify-start",
            );
          }}
          to={item.href}
        >
          {item.title}
        </NavLink>
      ))}
    </nav>
  );
}
