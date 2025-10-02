'use client';

import { motion, AnimatePresence } from 'framer-motion';
import { Sparkles } from 'lucide-react';

export default function MissionCompleteDialog({ isOpen, mission, onClose }) {
  if (!isOpen || !mission) return null;

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          className="fixed bottom-0 left-0 right-0 z-50 flex justify-center pb-24 md:pb-8 px-4"
          initial={{ y: 200, opacity: 0 }}
          animate={{ y: 0, opacity: 1 }}
          exit={{ y: 200, opacity: 0 }}
          transition={{ type: 'spring', damping: 25, stiffness: 300 }}
        >
          <div className="relative w-full max-w-4xl bg-gradient-to-br from-slate-900/98 to-slate-800/98 backdrop-blur-md rounded-2xl border-2 border-blue-500/50 shadow-2xl shadow-blue-500/30 overflow-hidden">
            <div className="absolute inset-0 bg-gradient-to-r from-blue-500/10 via-purple-500/10 to-pink-500/10 animate-pulse" />

            <motion.div
              className="absolute top-0 left-0 right-0 h-1 bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500"
              initial={{ scaleX: 0 }}
              animate={{ scaleX: 1 }}
              transition={{ duration: 0.8, ease: 'easeOut' }}
            />

            <div className="relative flex items-center gap-6 p-6">
              <motion.div
                className="hidden md:block flex-shrink-0"
                initial={{ scale: 0.8, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                transition={{ delay: 0.2 }}
              >
                <div className="relative">
                  <motion.div
                    className="absolute inset-0 bg-gradient-to-r from-blue-500 to-purple-500 rounded-xl blur-xl opacity-50"
                    animate={{ scale: [1, 1.1, 1] }}
                    transition={{ duration: 2, repeat: Infinity }}
                  />
                  <img
                    src="/images/photo_2025-10-01_21-30-27.png"
                    alt="Commander"
                    className="relative w-48 h-48 object-cover object-top rounded-xl border-2 border-blue-400/50"
                  />
                </div>
              </motion.div>

              <div className="flex-1">
                <motion.div
                  initial={{ x: -20, opacity: 0 }}
                  animate={{ x: 0, opacity: 1 }}
                  transition={{ delay: 0.3 }}
                  className="flex items-center gap-3 mb-3"
                >
                  <motion.div
                    animate={{ rotate: [0, 10, -10, 10, 0] }}
                    transition={{ duration: 0.5, delay: 0.5 }}
                  >
                    <Sparkles className="w-8 h-8 text-yellow-400" />
                  </motion.div>
                  <h2 className="text-3xl font-bold bg-gradient-to-r from-blue-400 via-purple-400 to-pink-400 bg-clip-text text-transparent">
                    Миссия выполнена!
                  </h2>
                </motion.div>

                <motion.p
                  className="text-lg text-slate-300 mb-4"
                  initial={{ x: -20, opacity: 0 }}
                  animate={{ x: 0, opacity: 1 }}
                  transition={{ delay: 0.4 }}
                >
                  Отличная работа, космонавт! Вы успешно завершили миссию "{mission.title}".
                  Продолжайте в том же духе!
                </motion.p>

                <motion.div
                  className="flex flex-wrap gap-3"
                  initial={{ y: 20, opacity: 0 }}
                  animate={{ y: 0, opacity: 1 }}
                  transition={{ delay: 0.5 }}
                >
                  <div className="flex items-center gap-2 px-4 py-2 bg-blue-500/20 rounded-lg border border-blue-500/50">
                    <span className="text-2xl">⭐</span>
                    <span className="font-bold text-white">+{mission.experienceReward} опыта</span>
                  </div>
                  <div className="flex items-center gap-2 px-4 py-2 bg-purple-500/20 rounded-lg border border-purple-500/50">
                    <span className="text-2xl">⚡</span>
                    <span className="font-bold text-white">+{mission.manaReward} маны</span>
                  </div>
                </motion.div>
              </div>

              <motion.div
                className="md:hidden absolute top-4 right-4"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                transition={{ delay: 0.2 }}
              >
                <img
                  src="/images/photo_2025-10-01_21-30-27.png"
                  alt="Commander"
                  className="w-20 h-20 object-cover object-top rounded-lg border-2 border-blue-400/50"
                />
              </motion.div>
            </div>
          </div>
        </motion.div>
      )}
    </AnimatePresence>
  );
}