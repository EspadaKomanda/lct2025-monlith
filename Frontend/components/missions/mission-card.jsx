'use client';

import { motion } from 'framer-motion';
import { Star, Zap, CircleCheck as CheckCircle, Lock } from 'lucide-react';
import * as Icons from 'lucide-react';
import CosmicCard from '@/components/ui/cosmic-card';

export default function MissionCard({ mission, onClick, userRank }) {
  const Icon = Icons[mission.icon] || Icons.Target;
  const isLocked = !canAccessMission(userRank, mission.requiredRank);
  const isCompleted = mission.status === 'completed';

  return (
    <CosmicCard
      onClick={!isLocked ? onClick : undefined}
      className={`p-5 ${isLocked ? 'opacity-60' : ''}`}
    >
      <div className="flex items-start gap-4">
        <div
          className={`p-3 rounded-lg ${
            isCompleted
              ? 'bg-green-500/20'
              : isLocked
              ? 'bg-slate-700'
              : 'bg-blue-500/20'
          }`}
        >
          {isCompleted ? (
            <CheckCircle className="w-6 h-6 text-green-400" />
          ) : isLocked ? (
            <Lock className="w-6 h-6 text-slate-400" />
          ) : (
            <Icon className="w-6 h-6 text-blue-400" />
          )}
        </div>

        <div className="flex-1">
          <div className="flex items-start justify-between mb-2">
            <div>
              <h3 className="text-lg font-bold text-white mb-1">{mission.title}</h3>
              <span className="inline-block px-2 py-1 text-xs font-medium bg-purple-500/20 text-purple-400 rounded">
                {mission.category}
              </span>
            </div>
          </div>

          <p className="text-sm text-slate-400 mb-3 line-clamp-2">{mission.description}</p>

          <div className="flex items-center gap-4 text-sm">
            <div className="flex items-center gap-1 text-blue-400">
              <Star className="w-4 h-4" />
              <span className="font-semibold">+{mission.experienceReward}</span>
            </div>
            <div className="flex items-center gap-1 text-purple-400">
              <Zap className="w-4 h-4" />
              <span className="font-semibold">+{mission.manaReward}</span>
            </div>
          </div>

          {isLocked && (
            <motion.div
              className="mt-3 p-2 bg-slate-800/50 rounded text-xs text-slate-400 flex items-center gap-2"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
            >
              <Lock className="w-3 h-3" />
              Требуется ранг: {mission.requiredRank}
            </motion.div>
          )}
        </div>
      </div>
    </CosmicCard>
  );
}

function canAccessMission(userRank, requiredRank) {
  const ranks = [
    'Искатель',
    'Пилот-кандидат',
    'Младший пилот',
    'Пилот',
    'Старший пилот',
    'Капитан',
    'Командир',
    'Адмирал',
  ];
  const userRankIndex = ranks.indexOf(userRank);
  const requiredRankIndex = ranks.indexOf(requiredRank);
  return userRankIndex >= requiredRankIndex;
}