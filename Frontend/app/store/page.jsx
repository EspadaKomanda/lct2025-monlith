'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { ShoppingBag, Zap, Check, X, Filter } from 'lucide-react';
import useStore from '@/lib/store';
import MobileNav from '@/components/navigation/mobile-nav';
import CosmicCard from '@/components/ui/cosmic-card';
import Modal from '@/components/ui/modal';
import { storeCategories } from '@/lib/mockData';
import { toast } from 'sonner';

export default function StorePage() {
  const router = useRouter();
  const { user, storeItems, purchaseItem } = useStore();
  const [selectedCategory, setSelectedCategory] = useState('Все');
  const [selectedItem, setSelectedItem] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user) return null;

  const filteredItems = storeItems.filter(
    (item) => selectedCategory === 'Все' || item.category === selectedCategory
  );

  const handlePurchase = () => {
    if (selectedItem && user.mana >= selectedItem.price) {
      const success = purchaseItem(selectedItem.id);
      if (success) {
        toast.success('Покупка успешна!', {
          description: `Вы приобрели ${selectedItem.name}`,
        });
        setIsModalOpen(false);
        setSelectedItem(null);
      }
    } else {
      toast.error('Недостаточно маны', {
        description: 'Выполняйте миссии, чтобы заработать больше маны',
      });
    }
  };

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

        <div className="flex items-center justify-between mb-8">
          <div>
            <div className="flex items-center gap-3 mb-2">
              <ShoppingBag className="w-8 h-8 text-blue-400" />
              <h1 className="text-3xl font-bold text-white">Магазин</h1>
            </div>
            <p className="text-slate-400">Обменяйте ману на награды</p>
          </div>

          <div className="flex items-center gap-2 px-4 py-2 bg-gradient-to-r from-purple-500/20 to-purple-600/20 border border-purple-500/50 rounded-lg">
            <Zap className="w-5 h-5 text-purple-400" />
            <span className="text-xl font-bold text-white">{user.mana}</span>
          </div>
        </div>

        <div className="mb-6">
          <div className="flex items-center gap-2 mb-3">
            <Filter className="w-5 h-5 text-slate-400" />
            <span className="text-sm font-medium text-slate-300">Категория</span>
          </div>
          <div className="flex items-center gap-2 overflow-x-auto pb-2">
            {storeCategories.map((category) => (
              <motion.button
                key={category}
                onClick={() => setSelectedCategory(category)}
                className={`px-4 py-2 rounded-lg font-medium text-sm whitespace-nowrap transition-all ${
                  selectedCategory === category
                    ? 'bg-purple-500 text-white'
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

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredItems.map((item, index) => (
            <motion.div
              key={item.id}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: index * 0.1 }}
            >
              <CosmicCard
                className="overflow-hidden"
                onClick={() => {
                  setSelectedItem(item);
                  setIsModalOpen(true);
                }}
              >
                <img
                  src={item.image}
                  alt={item.name}
                  className="w-full h-48 object-cover"
                />
                <div className="p-5">
                  <h3 className="text-lg font-bold text-white mb-1">{item.name}</h3>
                  <span className="inline-block px-2 py-1 text-xs font-medium bg-purple-500/20 text-purple-400 rounded mb-3">
                    {item.category}
                  </span>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Zap className="w-5 h-5 text-purple-400" />
                      <span className="text-xl font-bold text-white">{item.price}</span>
                    </div>
                    <button
                      className={`px-4 py-2 rounded-lg font-medium text-sm transition-colors ${
                        user.mana >= item.price
                          ? 'bg-purple-500 text-white hover:bg-purple-600'
                          : 'bg-slate-700 text-slate-400 cursor-not-allowed'
                      }`}
                      onClick={(e) => {
                        e.stopPropagation();
                        setSelectedItem(item);
                        setIsModalOpen(true);
                      }}
                    >
                      Купить
                    </button>
                  </div>
                </div>
              </CosmicCard>
            </motion.div>
          ))}
        </div>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setSelectedItem(null);
        }}
        title="Подтверждение покупки"
      >
        {selectedItem && (
          <div>
            <img
              src={selectedItem.image}
              alt={selectedItem.name}
              className="w-full h-64 object-cover rounded-lg mb-4"
            />

            <h3 className="text-2xl font-bold text-white mb-2">{selectedItem.name}</h3>
            <p className="text-slate-400 mb-4">{selectedItem.description}</p>

            <div className="flex items-center justify-between p-4 bg-slate-800/50 rounded-lg border border-slate-700 mb-6">
              <span className="text-slate-300 font-medium">Стоимость:</span>
              <div className="flex items-center gap-2">
                <Zap className="w-5 h-5 text-purple-400" />
                <span className="text-2xl font-bold text-white">{selectedItem.price}</span>
              </div>
            </div>

            <div className="flex items-center justify-between p-4 bg-purple-500/10 rounded-lg border border-purple-500/30 mb-6">
              <span className="text-slate-300 font-medium">Ваш баланс:</span>
              <div className="flex items-center gap-2">
                <Zap className="w-5 h-5 text-purple-400" />
                <span className="text-2xl font-bold text-white">{user.mana}</span>
              </div>
            </div>

            {user.mana < selectedItem.price && (
              <div className="p-4 bg-red-500/10 border border-red-500/30 rounded-lg mb-6">
                <p className="text-red-400 text-sm">
                  Недостаточно маны. Необходимо еще {selectedItem.price - user.mana} маны.
                </p>
              </div>
            )}

            <div className="flex gap-3">
              <button
                onClick={() => {
                  setIsModalOpen(false);
                  setSelectedItem(null);
                }}
                className="flex-1 py-3 px-4 bg-slate-700 hover:bg-slate-600 text-white font-semibold rounded-lg transition-colors flex items-center justify-center gap-2"
              >
                <X className="w-5 h-5" />
                Отмена
              </button>
              <button
                onClick={handlePurchase}
                disabled={user.mana < selectedItem.price}
                className="flex-1 py-3 px-4 bg-gradient-to-r from-purple-500 to-pink-600 text-white font-semibold rounded-lg hover:from-purple-600 hover:to-pink-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all flex items-center justify-center gap-2"
              >
                <Check className="w-5 h-5" />
                Купить
              </button>
            </div>
          </div>
        )}
      </Modal>

      <MobileNav />
    </div>
  );
}