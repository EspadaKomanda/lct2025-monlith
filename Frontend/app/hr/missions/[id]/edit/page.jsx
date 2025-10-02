'use client';

import { useEffect, useState } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { motion } from 'framer-motion';
import { ArrowLeft, Save } from 'lucide-react';
import useStore from '@/lib/store';
import HRSidebar from '@/components/navigation/hr-sidebar';
import CosmicCard from '@/components/ui/cosmic-card';
import { categories, ranks } from '@/lib/mockData';
import { toast } from 'sonner';

const icons = [
  'FileText',
  'Users',
  'Code',
  'Presentation',
  'BookOpen',
  'Lightbulb',
  'Rocket',
  'TrendingUp',
  'Target',
  'Award',
];

export default function EditMissionPage() {
  const params = useParams();
  const router = useRouter();
  const { user, isHR, missions, updateMission } = useStore();
  const [formData, setFormData] = useState(null);

  const mission = missions.find((m) => m.id === parseInt(params.id));

  useEffect(() => {
    if (!user) {
      router.push('/login');
    } else if (!isHR) {
      router.push('/dashboard');
    } else if (mission) {
      setFormData({
        title: mission.title,
        description: mission.description,
        category: mission.category,
        experienceReward: mission.experienceReward,
        manaReward: mission.manaReward,
        requiredRank: mission.requiredRank,
        icon: mission.icon,
        skills: mission.skills || [],
      });
    }
  }, [user, isHR, mission, router]);

  if (!user || !isHR || !mission || !formData) return null;

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.title || !formData.description) {
      toast.error('Заполните все обязательные поля');
      return;
    }

    updateMission(mission.id, formData);
    toast.success('Миссия обновлена успешно!');
    router.push('/hr/missions');
  };

  const handleChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <div className="flex min-h-screen cosmic-bg">
      <HRSidebar />

      <main className="flex-1 p-8">
        <div className="stars-bg fixed inset-0 pointer-events-none" />

        <div className="relative z-10 max-w-4xl mx-auto">
          <button
            onClick={() => router.back()}
            className="flex items-center gap-2 text-slate-400 hover:text-white mb-6 transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
            <span>Назад</span>
          </button>

          <div className="mb-8">
            <h1 className="text-3xl font-bold text-white mb-2">Редактирование миссии</h1>
            <p className="text-slate-400">Внесите изменения в миссию</p>
          </div>

          <CosmicCard className="p-8">
            <form onSubmit={handleSubmit} className="space-y-6">
              <div>
                <label className="block text-sm font-medium text-slate-300 mb-2">
                  Название миссии *
                </label>
                <input
                  type="text"
                  value={formData.title}
                  onChange={(e) => handleChange('title', e.target.value)}
                  placeholder="Например: Загрузить резюме"
                  className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white placeholder-slate-400 focus:outline-none focus:border-blue-500 transition-colors"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-slate-300 mb-2">
                  Описание *
                </label>
                <textarea
                  value={formData.description}
                  onChange={(e) => handleChange('description', e.target.value)}
                  placeholder="Подробное описание миссии..."
                  rows={4}
                  className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white placeholder-slate-400 focus:outline-none focus:border-blue-500 transition-colors resize-none"
                  required
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Категория
                  </label>
                  <select
                    value={formData.category}
                    onChange={(e) => handleChange('category', e.target.value)}
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  >
                    {categories.filter((c) => c !== 'Все').map((category) => (
                      <option key={category} value={category}>
                        {category}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">Иконка</label>
                  <select
                    value={formData.icon}
                    onChange={(e) => handleChange('icon', e.target.value)}
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  >
                    {icons.map((icon) => (
                      <option key={icon} value={icon}>
                        {icon}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Награда: Опыт
                  </label>
                  <input
                    type="number"
                    value={formData.experienceReward}
                    onChange={(e) => handleChange('experienceReward', parseInt(e.target.value))}
                    min="0"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-slate-300 mb-2">
                    Награда: Мана
                  </label>
                  <input
                    type="number"
                    value={formData.manaReward}
                    onChange={(e) => handleChange('manaReward', parseInt(e.target.value))}
                    min="0"
                    className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-slate-300 mb-2">
                  Требуемый ранг
                </label>
                <select
                  value={formData.requiredRank}
                  onChange={(e) => handleChange('requiredRank', e.target.value)}
                  className="w-full px-4 py-3 bg-slate-800 border border-slate-700 rounded-lg text-white focus:outline-none focus:border-blue-500 transition-colors"
                >
                  {ranks.map((rank) => (
                    <option key={rank} value={rank}>
                      {rank}
                    </option>
                  ))}
                </select>
              </div>

              <div className="pt-6 border-t border-slate-700 flex gap-4">
                <button
                  type="button"
                  onClick={() => router.back()}
                  className="flex-1 py-3 px-4 bg-slate-700 hover:bg-slate-600 text-white font-semibold rounded-lg transition-colors"
                >
                  Отмена
                </button>
                <motion.button
                  type="submit"
                  className="flex-1 py-3 px-4 bg-gradient-to-r from-blue-500 to-purple-600 text-white font-semibold rounded-lg hover:from-blue-600 hover:to-purple-700 transition-all flex items-center justify-center gap-2"
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                >
                  <Save className="w-5 h-5" />
                  Сохранить изменения
                </motion.button>
              </div>
            </form>
          </CosmicCard>
        </div>
      </main>
    </div>
  );
}