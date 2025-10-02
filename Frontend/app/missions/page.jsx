'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Filter } from 'lucide-react';
import useStore from '@/lib/store';
import MobileNav from '@/components/navigation/mobile-nav';
import MissionCard from '@/components/missions/mission-card';
import { categories } from '@/lib/mockData';

export default function MissionsPage() {
  const router = useRouter();
  const { user, missions } = useStore();
  const [selectedCategory, setSelectedCategory] = useState('Все');

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user) return null;

  const filteredMissions = missions.filter(
    (mission) => selectedCategory === 'Все' || mission.category === selectedCategory
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
          <h1 className="text-3xl font-bold text-white mb-1">Миссии</h1>
          <p className="text-slate-400">Выполняйте задания и получайте награды</p>
        </div>

        <div className="mb-6">
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

        <div className="space-y-4">
          {filteredMissions.length > 0 ? (
            filteredMissions.map((mission, index) => (
              <motion.div
                key={mission.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3, delay: index * 0.1 }}
              >
                <MissionCard
                  mission={mission}
                  onClick={() => router.push(`/missions/${mission.id}`)}
                  userRank={user.rank}
                />
              </motion.div>
            ))
          ) : (
            <div className="text-center py-12">
              <p className="text-slate-400">Нет доступных миссий в этой категории</p>
            </div>
          )}
        </div>
      </div>

      <MobileNav />
    </div>
  );
}