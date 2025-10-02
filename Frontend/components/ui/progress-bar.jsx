'use client';

import { motion } from 'framer-motion';

export default function ProgressBar({ value, max, className = '', label, showPercentage = false }) {
  const percentage = (value / max) * 100;

  return (
    <div className={`w-full ${className}`}>
      {label && (
        <div className="flex justify-between items-center mb-2">
          <span className="text-sm text-slate-300">{label}</span>
          {showPercentage && (
            <span className="text-sm text-blue-400 font-semibold">{Math.round(percentage)}%</span>
          )}
        </div>
      )}
      <div className="w-full bg-slate-800 rounded-full h-3 overflow-hidden border border-slate-700">
        <motion.div
          className="h-full bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 rounded-full"
          initial={{ width: 0 }}
          animate={{ width: `${percentage}%` }}
          transition={{ duration: 0.5, ease: 'easeOut' }}
        />
      </div>
    </div>
  );
}