'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { BookOpen, Trophy, Star, Zap, Clock } from 'lucide-react';
import * as Icons from 'lucide-react';
import useStore from '@/lib/store';
import MobileNav from '@/components/navigation/mobile-nav';
import CosmicCard from '@/components/ui/cosmic-card';

export default function JournalPage() {
  const router = useRouter();
  const { user, completedMissions, inventory } = useStore();

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user) return null;

  const sortedMissions = [...completedMissions].sort(
    (a, b) => new Date(b.completedAt) - new Date(a.completedAt)
  );

  return (
    <div className="min-h-screen pb-20 md:pb-8 cosmic-bg">
      <div className="stars-bg fixed inset-0 pointer-events-none" />

      <div className="relative z-10 max-w-7xl mx-auto px-4 py-6">
        <button
          onClick={() => router.back()}
          className="flex items-center gap-2 text-slate-400 hover:text-white mb-6 transition-colors"
        >
          <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
          </svg>
          <span>Назад</span>
        </button>

        <div className="mb-8">
          <div className="flex items-center gap-3 mb-2">
            <BookOpen className="w-8 h-8 text-blue-400" />
            <h1 className="text-3xl font-bold text-white">Бортовой журнал</h1>
          </div>
          <p className="text-slate-400">История ваших достижений</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
          <CosmicCard className="p-6">
            <div className="flex items-center gap-3">
              <div className="p-3 bg-blue-500/20 rounded-lg">
                <Trophy className="w-6 h-6 text-blue-400" />
              </div>
              <div>
                <p className="text-sm text-slate-400">Завершено миссий</p>
                <p className="text-2xl font-bold text-white">{completedMissions.length}</p>
              </div>
            </div>
          </CosmicCard>

          <CosmicCard className="p-6">
            <div className="flex items-center gap-3">
              <div className="p-3 bg-purple-500/20 rounded-lg">
                <Star className="w-6 h-6 text-purple-400" />
              </div>
              <div>
                <p className="text-sm text-slate-400">Текущий опыт</p>
                <p className="text-2xl font-bold text-white">{user.experience}</p>
              </div>
            </div>
          </CosmicCard>

          <CosmicCard className="p-6">
            <div className="flex items-center gap-3">
              <div className="p-3 bg-pink-500/20 rounded-lg">
                <Zap className="w-6 h-6 text-pink-400" />
              </div>
              <div>
                <p className="text-sm text-slate-400">Предметов получено</p>
                <p className="text-2xl font-bold text-white">{inventory.length}</p>
              </div>
            </div>
          </CosmicCard>
        </div>

        <div className="mb-8">
          <h2 className="text-2xl font-bold text-white mb-4">Завершенные миссии</h2>
          {sortedMissions.length > 0 ? (
            <div className="space-y-4">
              {sortedMissions.map((mission, index) => {
                const Icon = Icons[mission.icon] || Icons.Target;
                return (
                  <motion.div
                    key={`${mission.id}-${index}`}
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ duration: 0.3, delay: index * 0.05 }}
                  >
                    <CosmicCard className="p-5">
                      <div className="flex items-start gap-4">
                        <div className="p-3 bg-green-500/20 rounded-lg">
                          <Icon className="w-6 h-6 text-green-400" />
                        </div>

                        <div className="flex-1">
                          <div className="flex items-start justify-between mb-2">
                            <div>
                              <h3 className="text-lg font-bold text-white mb-1">
                                {mission.title}
                              </h3>
                              <span className="inline-block px-2 py-1 text-xs font-medium bg-purple-500/20 text-purple-400 rounded">
                                {mission.category}
                              </span>
                            </div>
                          </div>

                          <div className="flex items-center gap-4 text-sm mb-2">
                            <div className="flex items-center gap-1 text-blue-400">
                              <Star className="w-4 h-4" />
                              <span>+{mission.experienceReward} опыта</span>
                            </div>
                            <div className="flex items-center gap-1 text-purple-400">
                              <Zap className="w-4 h-4" />
                              <span>+{mission.manaReward} маны</span>
                            </div>
                          </div>

                          <div className="flex items-center gap-2 text-xs text-slate-500">
                            <Clock className="w-3 h-3" />
                            <span>
                              Завершено:{' '}
                              {new Date(mission.completedAt).toLocaleDateString('ru-RU', {
                                day: 'numeric',
                                month: 'long',
                                year: 'numeric',
                                hour: '2-digit',
                                minute: '2-digit',
                              })}
                            </span>
                          </div>
                        </div>
                      </div>
                    </CosmicCard>
                  </motion.div>
                );
              })}
            </div>
          ) : (
            <CosmicCard className="p-12">
              <div className="text-center">
                <div className="inline-block p-4 bg-slate-800 rounded-full mb-4">
                  <Trophy className="w-8 h-8 text-slate-600" />
                </div>
                <p className="text-slate-400 mb-2">Вы еще не завершили ни одной миссии</p>
                <button
                  onClick={() => router.push('/missions')}
                  className="text-blue-400 hover:text-blue-300 font-medium"
                >
                  Перейти к миссиям
                </button>
              </div>
            </CosmicCard>
          )}
        </div>

        {inventory.length > 0 && (
          <div>
            <h2 className="text-2xl font-bold text-white mb-4">Полученные предметы</h2>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              {inventory.map((item, index) => (
                <motion.div
                  key={`${item.id}-${index}`}
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.3, delay: index * 0.05 }}
                >
                  <CosmicCard className="p-4">
                    <img
                      src={item.image}
                      alt={item.name}
                      className="w-full h-32 object-cover rounded-lg mb-3"
                    />
                    <h3 className="text-sm font-bold text-white mb-1">{item.name}</h3>
                    <p className="text-xs text-slate-400">{item.category}</p>
                  </CosmicCard>
                </motion.div>
              ))}
            </div>
          </div>
        )}
      </div>

      <MobileNav />
    </div>
  );
}