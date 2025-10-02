'use client';

import { motion } from 'framer-motion';

export default function CosmicCard({ children, className = '', animate = true, onClick, clickable = false }) {
  const Component = animate ? motion.div : 'div';
  const animationProps = animate && clickable
    ? {
        whileHover: { scale: 1.02, y: -4 },
        whileTap: { scale: 0.98 },
        transition: { duration: 0.2 },
      }
    : {};

  return (
    <Component
      className={`bg-gradient-to-br from-slate-900 to-slate-800 rounded-xl border border-slate-700 shadow-lg hover:shadow-blue-500/20 transition-shadow ${
        clickable && onClick ? 'cursor-pointer' : ''
      } ${className}`}
      onClick={onClick}
      {...animationProps}
    >
      {children}
    </Component>
  );
}