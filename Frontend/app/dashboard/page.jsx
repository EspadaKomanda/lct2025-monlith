'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Star, Zap, Trophy, ArrowRight, LogOut } from 'lucide-react';
import useStore from '@/lib/store';
import MobileNav from '@/components/navigation/mobile-nav';
import CosmicCard from '@/components/ui/cosmic-card';
import ProgressBar from '@/components/ui/progress-bar';
import StatCard from '@/components/ui/stat-card';

export default function DashboardPage() {
  const router = useRouter();
  const { user, missions, setUser } = useStore();

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user) return null;

  const availableMissions = missions.filter((m) => m.status === 'available').length;
  const completedMissions = missions.filter((m) => m.status === 'completed').length;

  const handleLogout = () => {
    setUser(null);
    router.push('/login');
  };

  return (
    <div className="min-h-screen pb-20 md:pb-8 cosmic-bg">
      <div className="stars-bg fixed inset-0 pointer-events-none" />

      <div className="relative z-10 max-w-7xl mx-auto px-4 py-6">
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-white mb-1">Главная</h1>
            <p className="text-slate-400">Добро пожаловать, {user.name}</p>
          </div>
          <button
            onClick={handleLogout}
            className="p-2 bg-slate-800 hover:bg-slate-700 rounded-lg transition-colors"
          >
            <LogOut className="w-5 h-5 text-slate-400" />
          </button>
        </div>

        <CosmicCard className="p-6 mb-6">
          <div className="flex items-center gap-4 mb-4">
            <div className="p-3 bg-gradient-to-br from-blue-500 to-purple-600 rounded-full">
              <Trophy className="w-8 h-8 text-white" />
            </div>
            <div>
              <h2 className="text-2xl font-bold text-white">{user.rank}</h2>
              <p className="text-sm text-slate-400">Следующий ранг: {user.nextRank}</p>
            </div>
          </div>

          <ProgressBar
            value={user.experience}
            max={user.neededExperience}
            label="Опыт до следующего ранга"
            showPercentage
          />

          <div className="flex items-center justify-between mt-2 text-sm">
            <span className="text-slate-400">
              {user.experience} / {user.neededExperience} XP
            </span>
          </div>
        </CosmicCard>

        <div className="grid grid-cols-2 gap-4 mb-6">
          <StatCard icon={Star} label="Опыт" value={user.experience} color="blue" />
          <StatCard icon={Zap} label="Мана" value={user.mana} color="purple" />
        </div>

        <CosmicCard className="p-6 mb-6">
          <h3 className="text-xl font-bold text-white mb-4">Навыки</h3>
          <div className="space-y-4">
            {user.skills.map((skill) => (
              <motion.div
                key={skill.id}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.3 }}
              >
                <div className="flex items-center justify-between mb-2">
                  <span className="text-slate-300 font-medium">{skill.name}</span>
                  <span className="text-blue-400 font-semibold">Уровень {skill.level}</span>
                </div>
                <ProgressBar value={skill.progress} max={100} />
              </motion.div>
            ))}
          </div>
        </CosmicCard>

        <CosmicCard className="p-6 mb-6">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-xl font-bold text-white">Статистика</h3>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="p-4 bg-slate-800/50 rounded-lg border border-slate-700">
              <p className="text-sm text-slate-400 mb-1">Доступно миссий</p>
              <p className="text-2xl font-bold text-green-400">{availableMissions}</p>
            </div>
            <div className="p-4 bg-slate-800/50 rounded-lg border border-slate-700">
              <p className="text-sm text-slate-400 mb-1">Завершено миссий</p>
              <p className="text-2xl font-bold text-blue-400">{completedMissions}</p>
            </div>
          </div>
        </CosmicCard>

        <motion.button
          onClick={() => router.push('/missions')}
          className="w-full p-4 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg font-semibold text-white flex items-center justify-center gap-2 hover:from-blue-600 hover:to-purple-700 transition-all"
          whileHover={{ scale: 1.02 }}
          whileTap={{ scale: 0.98 }}
        >
          Перейти к миссиям
          <ArrowRight className="w-5 h-5" />
        </motion.button>
      </div>

      <MobileNav />
    </div>
  );
}