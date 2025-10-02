'use client';

import { motion } from 'framer-motion';

export default function StatCard({ icon: Icon, label, value, color = 'blue' }) {
  const colorMap = {
    blue: 'from-blue-500/20 to-blue-600/20 border-blue-500/50 text-blue-400',
    purple: 'from-purple-500/20 to-purple-600/20 border-purple-500/50 text-purple-400',
    pink: 'from-pink-500/20 to-pink-600/20 border-pink-500/50 text-pink-400',
    green: 'from-green-500/20 to-green-600/20 border-green-500/50 text-green-400',
  };

  return (
    <motion.div
      className={`bg-gradient-to-br ${colorMap[color]} rounded-lg p-4 border`}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3 }}
    >
      <div className="flex items-center gap-3">
        <div className="p-2 bg-slate-900/50 rounded-lg">
          <Icon className="w-6 h-6" />
        </div>
        <div>
          <p className="text-xs text-slate-400">{label}</p>
          <p className="text-2xl font-bold text-white">{value}</p>
        </div>
      </div>
    </motion.div>
  );
}