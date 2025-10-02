'use client';

import { useEffect, useState } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { motion } from 'framer-motion';
import { ArrowLeft, Star, Zap, CircleCheck as CheckCircle, Trophy } from 'lucide-react';
import * as Icons from 'lucide-react';
import useStore from '@/lib/store';
import CosmicCard from '@/components/ui/cosmic-card';
import MissionCompleteDialog from '@/components/ui/mission-complete-dialog';
import { toast } from 'sonner';

export default function MissionDetailPage() {
  const params = useParams();
  const router = useRouter();
  const { user, missions, completeMission } = useStore();
  const [isCompleting, setIsCompleting] = useState(false);
  const [showReward, setShowReward] = useState(false);
  const [showDialog, setShowDialog] = useState(false);

  const mission = missions.find((m) => m.id === parseInt(params.id));

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user || !mission) {
    return null;
  }

  const Icon = Icons[mission.icon] || Icons.Target;
  const isCompleted = mission.status === 'completed';

  const handleCompleteMission = async () => {
    if (isCompleted) return;

    setIsCompleting(true);

    setTimeout(() => {
      completeMission(mission.id);
      setIsCompleting(false);
      setShowReward(true);
      setShowDialog(true);

      setTimeout(() => {
        setShowDialog(false);
        setTimeout(() => {
          router.push('/missions');
        }, 500);
      }, 4000);
    }, 1500);
  };

  return (
    <div className="min-h-screen pb-20 md:pb-8 cosmic-bg">
      <div className="stars-bg fixed inset-0 pointer-events-none" />

      <div className="relative z-10 max-w-3xl mx-auto px-4 py-6">
        <button
          onClick={() => router.back()}
          className="flex items-center gap-2 text-slate-400 hover:text-white mb-6 transition-colors"
        >
          <ArrowLeft className="w-5 h-5" />
          <span>Назад</span>
        </button>

        <CosmicCard className="p-6 mb-6">
          <div className="flex items-start gap-4 mb-6">
            <div
              className={`p-4 rounded-lg ${
                isCompleted ? 'bg-green-500/20' : 'bg-blue-500/20'
              }`}
            >
              {isCompleted ? (
                <CheckCircle className="w-8 h-8 text-green-400" />
              ) : (
                <Icon className="w-8 h-8 text-blue-400" />
              )}
            </div>

            <div className="flex-1">
              <div className="flex items-start justify-between mb-2">
                <h1 className="text-2xl font-bold text-white">{mission.title}</h1>
                {isCompleted && (
                  <span className="px-3 py-1 bg-green-500/20 text-green-400 rounded-full text-sm font-medium">
                    Завершено
                  </span>
                )}
              </div>
              <span className="inline-block px-3 py-1 text-sm font-medium bg-purple-500/20 text-purple-400 rounded">
                {mission.category}
              </span>
            </div>
          </div>

          <div className="mb-6">
            <h2 className="text-lg font-semibold text-white mb-2">Описание</h2>
            <p className="text-slate-300 leading-relaxed">{mission.description}</p>
          </div>

          <div className="mb-6">
            <h2 className="text-lg font-semibold text-white mb-3">Требования</h2>
            <div className="flex items-center gap-2 p-3 bg-slate-800/50 rounded-lg border border-slate-700">
              <Trophy className="w-5 h-5 text-blue-400" />
              <span className="text-slate-300">Требуемый ранг: {mission.requiredRank}</span>
            </div>
          </div>

          <div className="mb-6">
            <h2 className="text-lg font-semibold text-white mb-3">Награды</h2>
            <div className="grid grid-cols-2 gap-4">
              <motion.div
                className="p-4 bg-gradient-to-br from-blue-500/20 to-blue-600/20 rounded-lg border border-blue-500/50"
                animate={showReward ? { scale: [1, 1.1, 1] } : {}}
                transition={{ duration: 0.5 }}
              >
                <div className="flex items-center gap-3">
                  <Star className="w-6 h-6 text-blue-400" />
                  <div>
                    <p className="text-xs text-slate-400">Опыт</p>
                    <p className="text-xl font-bold text-white">+{mission.experienceReward}</p>
                  </div>
                </div>
              </motion.div>

              <motion.div
                className="p-4 bg-gradient-to-br from-purple-500/20 to-purple-600/20 rounded-lg border border-purple-500/50"
                animate={showReward ? { scale: [1, 1.1, 1] } : {}}
                transition={{ duration: 0.5, delay: 0.1 }}
              >
                <div className="flex items-center gap-3">
                  <Zap className="w-6 h-6 text-purple-400" />
                  <div>
                    <p className="text-xs text-slate-400">Мана</p>
                    <p className="text-xl font-bold text-white">+{mission.manaReward}</p>
                  </div>
                </div>
              </motion.div>
            </div>

            {mission.skills && mission.skills.length > 0 && (
              <div className="mt-4 p-4 bg-slate-800/50 rounded-lg border border-slate-700">
                <p className="text-sm text-slate-400 mb-2">Прокачиваемые навыки:</p>
                <div className="flex flex-wrap gap-2">
                  {mission.skills.map((skill) => (
                    <span
                      key={skill.skillId}
                      className="px-3 py-1 bg-green-500/20 text-green-400 rounded-full text-sm"
                    >
                      +{skill.value} уровня
                    </span>
                  ))}
                </div>
              </div>
            )}
          </div>

          {!isCompleted && (
            <motion.button
              onClick={handleCompleteMission}
              disabled={isCompleting}
              className="w-full py-4 bg-gradient-to-r from-blue-500 to-purple-600 text-white font-semibold rounded-lg hover:from-blue-600 hover:to-purple-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
              whileHover={{ scale: isCompleting ? 1 : 1.02 }}
              whileTap={{ scale: isCompleting ? 1 : 0.98 }}
            >
              {isCompleting ? 'Выполняется...' : 'Выполнить миссию'}
            </motion.button>
          )}
        </CosmicCard>
      </div>

      <MissionCompleteDialog isOpen={showDialog} mission={mission} onClose={() => setShowDialog(false)} />
    </div>
  );
}