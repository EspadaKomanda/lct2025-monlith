'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Users, Star, Zap, Trophy } from 'lucide-react';
import useStore from '@/lib/store';
import HRSidebar from '@/components/navigation/hr-sidebar';
import CosmicCard from '@/components/ui/cosmic-card';
import { mockUsers } from '@/lib/mockData';

export default function HRUsersPage() {
  const router = useRouter();
  const { user, isHR } = useStore();

  useEffect(() => {
    if (!user) {
      router.push('/login');
    } else if (!isHR) {
      router.push('/dashboard');
    }
  }, [user, isHR, router]);

  if (!user || !isHR) return null;

  return (
    <div className="flex min-h-screen cosmic-bg">
      <HRSidebar />

      <main className="flex-1 p-8">
        <div className="stars-bg fixed inset-0 pointer-events-none" />

        <div className="relative z-10 max-w-7xl mx-auto">
          <div className="mb-8">
            <div className="flex items-center gap-3 mb-2">
              <Users className="w-8 h-8 text-blue-400" />
              <h1 className="text-3xl font-bold text-white">Пользователи</h1>
            </div>
            <p className="text-slate-400">Просмотр статистики пользователей</p>
          </div>

          <div className="space-y-6">
            {mockUsers.map((userData, index) => (
              <motion.div
                key={userData.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3, delay: index * 0.1 }}
              >
                <CosmicCard className="p-6">
                  <div className="flex items-start justify-between mb-6">
                    <div>
                      <h2 className="text-2xl font-bold text-white mb-1">{userData.name}</h2>
                      <div className="flex items-center gap-2">
                        <Trophy className="w-5 h-5 text-blue-400" />
                        <span className="text-blue-400 font-semibold">{userData.rank}</span>
                        <span className="text-slate-500">→</span>
                        <span className="text-slate-400">{userData.nextRank}</span>
                      </div>
                    </div>

                    <div className="flex gap-4">
                      <div className="text-right">
                        <div className="flex items-center gap-2 justify-end mb-1">
                          <Star className="w-5 h-5 text-blue-400" />
                          <span className="text-xl font-bold text-white">
                            {userData.experience}
                          </span>
                        </div>
                        <p className="text-xs text-slate-400">Опыт</p>
                      </div>
                      <div className="text-right">
                        <div className="flex items-center gap-2 justify-end mb-1">
                          <Zap className="w-5 h-5 text-purple-400" />
                          <span className="text-xl font-bold text-white">{userData.mana}</span>
                        </div>
                        <p className="text-xs text-slate-400">Мана</p>
                      </div>
                    </div>
                  </div>

                  <div className="mb-4">
                    <div className="flex items-center justify-between mb-2">
                      <span className="text-sm text-slate-400">Прогресс до следующего ранга</span>
                      <span className="text-sm text-blue-400 font-semibold">
                        {Math.round((userData.experience / userData.neededExperience) * 100)}%
                      </span>
                    </div>
                    <div className="w-full bg-slate-800 rounded-full h-3 overflow-hidden border border-slate-700">
                      <div
                        className="h-full bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 rounded-full transition-all"
                        style={{
                          width: `${(userData.experience / userData.neededExperience) * 100}%`,
                        }}
                      />
                    </div>
                  </div>

                  <div>
                    <h3 className="text-lg font-semibold text-white mb-3">Навыки</h3>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                      {userData.skills.map((skill) => (
                        <div
                          key={skill.id}
                          className="p-3 bg-slate-800/50 rounded-lg border border-slate-700"
                        >
                          <div className="flex items-center justify-between mb-2">
                            <span className="text-sm text-slate-300 font-medium">
                              {skill.name}
                            </span>
                            <span className="text-sm text-blue-400 font-semibold">
                              Ур. {skill.level}
                            </span>
                          </div>
                          <div className="w-full bg-slate-700 rounded-full h-2 overflow-hidden">
                            <div
                              className="h-full bg-blue-500 rounded-full transition-all"
                              style={{ width: `${skill.progress}%` }}
                            />
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>
                </CosmicCard>
              </motion.div>
            ))}
          </div>
        </div>
      </main>
    </div>
  );
}