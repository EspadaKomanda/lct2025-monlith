'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Plus, Search, Filter, CreditCard as Edit, Trash2 } from 'lucide-react';
import * as Icons from 'lucide-react';
import useStore from '@/lib/store';
import HRSidebar from '@/components/navigation/hr-sidebar';
import CosmicCard from '@/components/ui/cosmic-card';
import { categories } from '@/lib/mockData';
import { toast } from 'sonner';

export default function HRMissionsPage() {
  const router = useRouter();
  const { user, missions, isHR, deleteMission } = useStore();
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('Все');

  useEffect(() => {
    if (!user) {
      router.push('/login');
    } else if (!isHR) {
      router.push('/dashboard');
    }
  }, [user, isHR, router]);

  if (!user || !isHR) return null;

  const filteredMissions = missions.filter((mission) => {
    const matchesSearch = mission.title.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesCategory = selectedCategory === 'Все' || mission.category === selectedCategory;
    return matchesSearch && matchesCategory;
  });

  const handleDelete = (missionId, title) => {
    if (confirm(`Вы уверены, что хотите удалить миссию "${title}"?`)) {
      deleteMission(missionId);
      toast.success('Миссия удалена');
    }
  };

  return (
    <div className="flex min-h-screen cosmic-bg">
      <HRSidebar />

      <main className="flex-1 p-8">
        <div className="stars-bg fixed inset-0 pointer-events-none" />

        <div className="relative z-10 max-w-7xl mx-auto">
          <div className="flex items-center justify-between mb-8">
            <div>
              <h1 className="text-3xl font-bold text-white mb-2">Управление миссиями</h1>
              <p className="text-slate-400">Создавайте и редактируйте миссии</p>
            </div>

            <motion.button
              onClick={() => router.push('/hr/missions/create')}
              className="px-6 py-3 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg font-semibold text-white hover:from-blue-600 hover:to-purple-700 transition-all flex items-center gap-2"
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              <Plus className="w-5 h-5" />
              Создать миссию
            </motion.button>
          </div>

          <div className="mb-6 space-y-4">
            <div className="relative">
              <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-400" />
              <input
                type="text"
                placeholder="Поиск миссий..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="w-full pl-12 pr-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white placeholder-slate-400 focus:outline-none focus:border-blue-500 transition-colors"
              />
            </div>

            <div>
              <div className="flex items-center gap-2 mb-3">
                <Filter className="w-5 h-5 text-slate-400" />
                <span className="text-sm font-medium text-slate-300">Категория</span>
              </div>
              <div className="flex items-center gap-2 overflow-x-auto pb-2">
                {categories.map((category) => (
                  <motion.button
                    key={category}
                    onClick={() => setSelectedCategory(category)}
                    className={`px-4 py-2 rounded-lg font-medium text-sm whitespace-nowrap transition-all ${
                      selectedCategory === category
                        ? 'bg-blue-500 text-white'
                        : 'bg-slate-800 text-slate-400 hover:bg-slate-700'
                    }`}
                    whileHover={{ scale: 1.05 }}
                    whileTap={{ scale: 0.95 }}
                  >
                    {category}
                  </motion.button>
                ))}
              </div>
            </div>
          </div>

          <div className="space-y-4">
            {filteredMissions.length > 0 ? (
              filteredMissions.map((mission, index) => {
                const Icon = Icons[mission.icon] || Icons.Target;
                return (
                  <motion.div
                    key={mission.id}
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.3, delay: index * 0.05 }}
                  >
                    <CosmicCard className="p-6">
                      <div className="flex items-start gap-4">
                        <div className="p-3 bg-blue-500/20 rounded-lg">
                          <Icon className="w-6 h-6 text-blue-400" />
                        </div>

                        <div className="flex-1">
                          <div className="flex items-start justify-between mb-2">
                            <div>
                              <h3 className="text-xl font-bold text-white mb-1">
                                {mission.title}
                              </h3>
                              <div className="flex items-center gap-2">
                                <span className="inline-block px-2 py-1 text-xs font-medium bg-purple-500/20 text-purple-400 rounded">
                                  {mission.category}
                                </span>
                                <span
                                  className={`inline-block px-2 py-1 text-xs font-medium rounded ${
                                    mission.status === 'completed'
                                      ? 'bg-green-500/20 text-green-400'
                                      : 'bg-blue-500/20 text-blue-400'
                                  }`}
                                >
                                  {mission.status === 'completed' ? 'Завершено' : 'Доступно'}
                                </span>
                              </div>
                            </div>

                            <div className="flex items-center gap-2">
                              <motion.button
                                onClick={() => router.push(`/hr/missions/${mission.id}/edit`)}
                                className="p-2 bg-slate-700 hover:bg-slate-600 rounded-lg transition-colors"
                                whileHover={{ scale: 1.1 }}
                                whileTap={{ scale: 0.9 }}
                              >
                                <Edit className="w-5 h-5 text-blue-400" />
                              </motion.button>

                              <motion.button
                                onClick={() => handleDelete(mission.id, mission.title)}
                                className="p-2 bg-slate-700 hover:bg-red-900/50 rounded-lg transition-colors"
                                whileHover={{ scale: 1.1 }}
                                whileTap={{ scale: 0.9 }}
                              >
                                <Trash2 className="w-5 h-5 text-red-400" />
                              </motion.button>
                            </div>
                          </div>

                          <p className="text-slate-400 mb-3">{mission.description}</p>

                          <div className="flex items-center gap-4 text-sm">
                            <div className="text-slate-300">
                              <span className="text-slate-500">Опыт:</span>{' '}
                              <span className="font-semibold text-blue-400">
                                +{mission.experienceReward}
                              </span>
                            </div>
                            <div className="text-slate-300">
                              <span className="text-slate-500">Мана:</span>{' '}
                              <span className="font-semibold text-purple-400">
                                +{mission.manaReward}
                              </span>
                            </div>
                            <div className="text-slate-300">
                              <span className="text-slate-500">Требуемый ранг:</span>{' '}
                              <span className="font-semibold">{mission.requiredRank}</span>
                            </div>
                          </div>
                        </div>
                      </div>
                    </CosmicCard>
                  </motion.div>
                );
              })
            ) : (
              <CosmicCard className="p-12">
                <div className="text-center">
                  <p className="text-slate-400">Миссии не найдены</p>
                </div>
              </CosmicCard>
            )}
          </div>
        </div>
      </main>
    </div>
  );
}