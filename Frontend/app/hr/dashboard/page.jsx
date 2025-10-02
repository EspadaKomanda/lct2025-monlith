'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Target, Users, CircleCheck as CheckCircle, Clock, TrendingUp, Star } from 'lucide-react';
import useStore from '@/lib/store';
import HRSidebar from '@/components/navigation/hr-sidebar';
import CosmicCard from '@/components/ui/cosmic-card';
import StatCard from '@/components/ui/stat-card';

export default function HRDashboardPage() {
  const router = useRouter();
  const { user, missions, isHR } = useStore();

  useEffect(() => {
    if (!user) {
      router.push('/login');
    } else if (!isHR) {
      router.push('/dashboard');
    }
  }, [user, isHR, router]);

  if (!user || !isHR) return null;

  const totalMissions = missions.length;
  const completedMissions = missions.filter((m) => m.status === 'completed').length;
  const availableMissions = missions.filter((m) => m.status === 'available').length;
  const completionRate = totalMissions > 0 ? Math.round((completedMissions / totalMissions) * 100) : 0;

  const missionsByCategory = missions.reduce((acc, mission) => {
    acc[mission.category] = (acc[mission.category] || 0) + 1;
    return acc;
  }, {});

  return (
    <div className="flex min-h-screen cosmic-bg">
      <HRSidebar />

      <main className="flex-1 p-8">
        <div className="stars-bg fixed inset-0 pointer-events-none" />

        <div className="relative z-10 max-w-7xl mx-auto">
          <div className="mb-8">
            <h1 className="text-3xl font-bold text-white mb-2">HR Дашборд</h1>
            <p className="text-slate-400">Управление системой геймификации</p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            <StatCard icon={Target} label="Всего миссий" value={totalMissions} color="blue" />
            <StatCard
              icon={CheckCircle}
              label="Завершено"
              value={completedMissions}
              color="green"
            />
            <StatCard icon={Clock} label="Доступно" value={availableMissions} color="purple" />
            <StatCard
              icon={TrendingUp}
              label="Процент завершения"
              value={`${completionRate}%`}
              color="pink"
            />
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
            <CosmicCard className="p-6">
              <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                <Target className="w-6 h-6 text-blue-400" />
                Миссии по категориям
              </h2>
              <div className="space-y-4">
                {Object.entries(missionsByCategory).map(([category, count], index) => (
                  <motion.div
                    key={category}
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ duration: 0.3, delay: index * 0.1 }}
                  >
                    <div className="flex items-center justify-between p-3 bg-slate-800/50 rounded-lg border border-slate-700">
                      <span className="text-slate-300 font-medium">{category}</span>
                      <span className="text-blue-400 font-bold">{count}</span>
                    </div>
                  </motion.div>
                ))}
              </div>
            </CosmicCard>

            <CosmicCard className="p-6">
              <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                <Star className="w-6 h-6 text-purple-400" />
                Быстрые действия
              </h2>
              <div className="space-y-3">
                <motion.button
                  onClick={() => router.push('/hr/missions/create')}
                  className="w-full p-4 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg font-semibold text-white hover:from-blue-600 hover:to-purple-700 transition-all text-left"
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                >
                  Создать новую миссию
                </motion.button>

                <motion.button
                  onClick={() => router.push('/hr/missions')}
                  className="w-full p-4 bg-slate-800 rounded-lg font-semibold text-white hover:bg-slate-700 transition-all text-left border border-slate-700"
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                >
                  Управление миссиями
                </motion.button>

                <motion.button
                  onClick={() => router.push('/hr/users')}
                  className="w-full p-4 bg-slate-800 rounded-lg font-semibold text-white hover:bg-slate-700 transition-all text-left border border-slate-700"
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                >
                  Просмотр пользователей
                </motion.button>
              </div>
            </CosmicCard>
          </div>

          <CosmicCard className="p-6">
            <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
              <CheckCircle className="w-6 h-6 text-green-400" />
              Последние завершенные миссии
            </h2>
            <div className="space-y-3">
              {missions
                .filter((m) => m.status === 'completed')
                .slice(0, 5)
                .map((mission, index) => (
                  <motion.div
                    key={mission.id}
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.3, delay: index * 0.05 }}
                    className="p-4 bg-slate-800/50 rounded-lg border border-slate-700"
                  >
                    <div className="flex items-center justify-between">
                      <div>
                        <p className="font-medium text-white">{mission.title}</p>
                        <p className="text-sm text-slate-400">{mission.category}</p>
                      </div>
                      <div className="flex items-center gap-4 text-sm">
                        <div className="flex items-center gap-1 text-blue-400">
                          <Star className="w-4 h-4" />
                          <span>+{mission.experienceReward}</span>
                        </div>
                      </div>
                    </div>
                  </motion.div>
                ))}
              {missions.filter((m) => m.status === 'completed').length === 0 && (
                <p className="text-center text-slate-400 py-8">
                  Пока нет завершенных миссий
                </p>
              )}
            </div>
          </CosmicCard>
        </div>
      </main>
    </div>
  );
}