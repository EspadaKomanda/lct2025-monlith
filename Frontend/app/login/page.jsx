'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Rocket, User } from 'lucide-react';
import useStore from '@/lib/store';
import { mockUsers, mockMissions, mockStoreItems } from '@/lib/mockData';

export default function LoginPage() {
  const router = useRouter();
  const { setUser, setMissions, setStoreItems, setIsHR } = useStore();
  const [selectedUser, setSelectedUser] = useState(null);
  const [isHRMode, setIsHRMode] = useState(false);

  const handleLogin = () => {
    if (selectedUser) {
      setUser(selectedUser);
      setMissions(mockMissions);
      setStoreItems(mockStoreItems);
      setIsHR(isHRMode);

      if (isHRMode) {
        router.push('/hr/dashboard');
      } else {
        router.push('/dashboard');
      }
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4 relative overflow-hidden">
      <div className="absolute inset-0 cosmic-bg" />
      <div className="absolute inset-0 stars-bg" />

      <motion.div
        className="relative z-10 w-full max-w-md bg-gradient-to-br from-slate-900/95 to-slate-800/95 backdrop-blur-sm rounded-2xl border border-slate-600/50 shadow-2xl shadow-blue-500/20 p-8"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="text-center mb-8">
          <motion.div
            className="inline-block p-4 bg-gradient-to-br from-blue-500/30 to-purple-500/30 rounded-full mb-4 shadow-lg shadow-blue-500/50"
            animate={{ rotate: 360 }}
            transition={{ duration: 20, repeat: Infinity, ease: 'linear' }}
          >
            <Rocket className="w-12 h-12 text-blue-300" />
          </motion.div>
          <h1 className="text-3xl font-bold bg-gradient-to-r from-blue-400 via-purple-400 to-pink-400 bg-clip-text text-transparent mb-2">
            Космический путь
          </h1>
          <p className="text-slate-300">Начните свое путешествие к звездам</p>
        </div>

        <div className="space-y-4 mb-6">
          <div>
            <label className="block text-sm font-medium text-slate-300 mb-3">
              Выберите пользователя
            </label>
            <div className="space-y-2">
              {mockUsers.map((user) => (
                <motion.button
                  key={user.id}
                  onClick={() => setSelectedUser(user)}
                  className={`w-full p-4 rounded-lg border-2 transition-all ${
                    selectedUser?.id === user.id
                      ? 'border-blue-400 bg-gradient-to-r from-blue-500/20 to-purple-500/20 shadow-lg shadow-blue-500/30'
                      : 'border-slate-600 bg-slate-800/80 hover:border-slate-500 hover:bg-slate-700/80'
                  }`}
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                >
                  <div className="flex items-center gap-3">
                    <div className="p-2 bg-slate-700 rounded-full">
                      <User className="w-5 h-5 text-slate-300" />
                    </div>
                    <div className="text-left">
                      <p className="font-semibold text-white">{user.name}</p>
                      <p className="text-sm text-slate-400">Ранг: {user.rank}</p>
                    </div>
                  </div>
                </motion.button>
              ))}
            </div>
          </div>

          <div className="flex items-center justify-between p-4 bg-slate-800/80 rounded-lg border border-slate-600">
            <label className="text-sm font-medium text-slate-200">Войти как HR</label>
            <button
              onClick={() => setIsHRMode(!isHRMode)}
              className={`relative w-12 h-6 rounded-full transition-colors ${
                isHRMode ? 'bg-gradient-to-r from-blue-500 to-purple-500' : 'bg-slate-600'
              }`}
            >
              <motion.div
                className="absolute top-1 w-4 h-4 bg-white rounded-full shadow-md"
                animate={{ left: isHRMode ? '26px' : '4px' }}
                transition={{ duration: 0.2 }}
              />
            </button>
          </div>
        </div>

        <motion.button
          onClick={handleLogin}
          disabled={!selectedUser}
          className="w-full py-3 px-4 bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 text-white font-semibold rounded-lg hover:from-blue-600 hover:via-purple-600 hover:to-pink-600 disabled:opacity-50 disabled:cursor-not-allowed transition-all shadow-lg shadow-blue-500/50"
          whileHover={{ scale: selectedUser ? 1.02 : 1 }}
          whileTap={{ scale: selectedUser ? 0.98 : 1 }}
        >
          {isHRMode ? 'Войти в HR панель' : 'Начать путешествие'}
        </motion.button>
      </motion.div>
    </div>
  );
}