'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { LayoutDashboard, Target, Users, Settings, ArrowLeft } from 'lucide-react';

export default function HRSidebar() {
  const pathname = usePathname();

  const navItems = [
    { href: '/hr/dashboard', icon: LayoutDashboard, label: 'Дашборд' },
    { href: '/hr/missions', icon: Target, label: 'Миссии' },
    { href: '/hr/users', icon: Users, label: 'Пользователи' },
    { href: '/hr/settings', icon: Settings, label: 'Настройки' },
  ];

  return (
    <aside className="hidden md:flex flex-col w-64 bg-slate-900 border-r border-slate-800 h-screen sticky top-0">
      <div className="p-6 border-b border-slate-800">
        <h1 className="text-xl font-bold text-white flex items-center gap-2">
          <span className="text-2xl">🚀</span>
          HR Панель
        </h1>
      </div>

      <nav className="flex-1 p-4 space-y-2">
        {navItems.map((item) => {
          const isActive = pathname === item.href;
          return (
            <Link
              key={item.href}
              href={item.href}
              className={`flex items-center gap-3 px-4 py-3 rounded-lg transition-colors ${
                isActive
                  ? 'bg-blue-500/20 text-blue-400 border border-blue-500/50'
                  : 'text-slate-400 hover:bg-slate-800 hover:text-slate-200'
              }`}
            >
              <item.icon className="w-5 h-5" />
              <span className="font-medium">{item.label}</span>
            </Link>
          );
        })}
      </nav>

      <div className="p-4 border-t border-slate-800">
        <Link
          href="/dashboard"
          className="flex items-center gap-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-800 hover:text-slate-200 transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
          <span className="font-medium">К профилю</span>
        </Link>
      </div>
    </aside>
  );
}