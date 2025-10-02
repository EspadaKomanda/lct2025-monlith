'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { Settings, Award, Zap, Star, Save } from 'lucide-react';
import { motion } from 'framer-motion';
import useStore from '@/lib/store';
import HRSidebar from '@/components/navigation/hr-sidebar';
import CosmicCard from '@/components/ui/cosmic-card';
import { toast } from 'sonner';

export default function HRSettingsPage() {
  const router = useRouter();
  const { user, isHR } = useStore();

  const [settings, setSettings] = useState({
    baseExperienceReward: 100,
    baseManaReward: 50,
    experienceMultiplier: 1.5,
    enableNotifications: true,
    enableLeaderboard: false,
    maxDailyMissions: 5,
    enableAchievements: true,
  });

  useEffect(() => {
    if (!user) {
      router.push('/login');
    } else if (!isHR) {
      router.push('/dashboard');
    }
  }, [user, isHR, router]);

  if (!user || !isHR) return null;

  const handleSave = () => {
    toast.success('Настройки сохранены', {
      description: 'Изменения будут применены ко всем пользователям',
    });
  };

  const handleChange = (key, value) => {
    setSettings((prev) => ({ ...prev, [key]: value }));
  };

  return (
    <div className="flex min-h-screen cosmic-bg">
      <HRSidebar />

      <main className="flex-1 p-8">
        <div className="stars-bg fixed inset-0 pointer-events-none" />

        <div className="relative z-10 max-w-4xl mx-auto">
          <div className="mb-8">
            <div className="flex items-center gap-3 mb-2">
              <Settings className="w-8 h-8 text-blue-400" />
              <h1 className="text-3xl font-bold text-white">Настройки системы</h1>
            </div>
            <p className="text-slate-400">Конфигурация параметров геймификации</p>
          </div>

          <div className="space-y-6">
            <CosmicCard className="p-6">
              <div className="flex items-center gap-3 mb-6">
                <Award className="w-6 h-6 text-blue-400" />
                <h2 className="text-xl font-bold text-white">Награды и опыт</h2>
              </div>

              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Базовая награда за опыт
                  </label>
                  <input
                    type="number"
                    value={settings.baseExperienceReward}
                    onChange={(e) =>
                      handleChange('baseExperienceReward', parseInt(e.target.value))
                    }
                    min="0"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                  <p className="text-xs text-slate-500 mt-1">
                    Минимальное количество опыта за выполнение миссии
                  </p>
                </div>

                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Базовая награда за ману
                  </label>
                  <input
                    type="number"
                    value={settings.baseManaReward}
                    onChange={(e) => handleChange('baseManaReward', parseInt(e.target.value))}
                    min="0"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                  <p className="text-xs text-slate-500 mt-1">
                    Минимальное количество маны за выполнение миссии
                  </p>
                </div>

                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Множитель опыта для повышения ранга
                  </label>
                  <input
                    type="number"
                    value={settings.experienceMultiplier}
                    onChange={(e) =>
                      handleChange('experienceMultiplier', parseFloat(e.target.value))
                    }
                    step="0.1"
                    min="1"
                    max="3"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                  <p className="text-xs text-slate-500 mt-1">
                    Коэффициент увеличения требуемого опыта для следующего ранга
                  </p>
                </div>
              </div>
            </CosmicCard>

            <CosmicCard className="p-6">
              <div className="flex items-center gap-3 mb-6">
                <Star className="w-6 h-6 text-purple-400" />
                <h2 className="text-xl font-bold text-white">Функциональность</h2>
              </div>

              <div className="space-y-4">
                <div className="flex items-center justify-between p-4 bg-slate-800/50 rounded-lg border border-slate-700">
                  <div>
                    <p className="text-slate-200 font-medium">Уведомления</p>
                    <p className="text-sm text-slate-400">
                      Отправлять уведомления о новых миссиях и достижениях
                    </p>
                  </div>
                  <button
                    onClick={() => handleChange('enableNotifications', !settings.enableNotifications)}
                    className={`relative w-12 h-6 rounded-full transition-colors ${
                      settings.enableNotifications
                        ? 'bg-gradient-to-r from-blue-500 to-purple-500'
                        : 'bg-slate-600'
                    }`}
                  >
                    <motion.div
                      className="absolute top-1 w-4 h-4 bg-white rounded-full shadow-md"
                      animate={{ left: settings.enableNotifications ? '26px' : '4px' }}
                      transition={{ duration: 0.2 }}
                    />
                  </button>
                </div>

                <div className="flex items-center justify-between p-4 bg-slate-800/50 rounded-lg border border-slate-700">
                  <div>
                    <p className="text-slate-200 font-medium">Таблица лидеров</p>
                    <p className="text-sm text-slate-400">
                      Показывать рейтинг пользователей по опыту
                    </p>
                  </div>
                  <button
                    onClick={() => handleChange('enableLeaderboard', !settings.enableLeaderboard)}
                    className={`relative w-12 h-6 rounded-full transition-colors ${
                      settings.enableLeaderboard
                        ? 'bg-gradient-to-r from-blue-500 to-purple-500'
                        : 'bg-slate-600'
                    }`}
                  >
                    <motion.div
                      className="absolute top-1 w-4 h-4 bg-white rounded-full shadow-md"
                      animate={{ left: settings.enableLeaderboard ? '26px' : '4px' }}
                      transition={{ duration: 0.2 }}
                    />
                  </button>
                </div>

                <div className="flex items-center justify-between p-4 bg-slate-800/50 rounded-lg border border-slate-700">
                  <div>
                    <p className="text-slate-200 font-medium">Достижения</p>
                    <p className="text-sm text-slate-400">
                      Включить систему достижений и значков
                    </p>
                  </div>
                  <button
                    onClick={() => handleChange('enableAchievements', !settings.enableAchievements)}
                    className={`relative w-12 h-6 rounded-full transition-colors ${
                      settings.enableAchievements
                        ? 'bg-gradient-to-r from-blue-500 to-purple-500'
                        : 'bg-slate-600'
                    }`}
                  >
                    <motion.div
                      className="absolute top-1 w-4 h-4 bg-white rounded-full shadow-md"
                      animate={{ left: settings.enableAchievements ? '26px' : '4px' }}
                      transition={{ duration: 0.2 }}
                    />
                  </button>
                </div>
              </div>
            </CosmicCard>

            <CosmicCard className="p-6">
              <div className="flex items-center gap-3 mb-6">
                <Zap className="w-6 h-6 text-pink-400" />
                <h2 className="text-xl font-bold text-white">Ограничения</h2>
              </div>

              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Максимум миссий в день
                  </label>
                  <input
                    type="number"
                    value={settings.maxDailyMissions}
                    onChange={(e) => handleChange('maxDailyMissions', parseInt(e.target.value))}
                    min="1"
                    max="20"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                  <p className="text-xs text-slate-500 mt-1">
                    Максимальное количество миссий, которые может выполнить пользователь за день
                  </p>
                </div>
              </div>
            </CosmicCard>

            <motion.button
              onClick={handleSave}
              className="w-full py-4 bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 text-white font-semibold rounded-lg hover:from-blue-600 hover:via-purple-600 hover:to-pink-600 transition-all shadow-lg shadow-blue-500/50 flex items-center justify-center gap-2"
              whileHover={{ scale: 1.02 }}
              whileTap={{ scale: 0.98 }}
            >
              <Save className="w-5 h-5" />
              Сохранить настройки
            </motion.button>
          </div>
        </div>
      </main>
    </div>
  );
}