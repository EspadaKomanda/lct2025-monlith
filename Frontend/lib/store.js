import { create } from 'zustand';

const useStore = create((set, get) => ({
  user: null,
  missions: [],
  skills: [],
  inventory: [],
  storeItems: [],
  completedMissions: [],
  isHR: false,

  setUser: (user) => set({ user }),

  setIsHR: (isHR) => set({ isHR }),

  setMissions: (missions) => set({ missions }),

  setSkills: (skills) => set({ skills }),

  setStoreItems: (items) => set({ storeItems: items }),

  completeMission: (missionId) => {
    const { missions, user, completedMissions } = get();
    const mission = missions.find((m) => m.id === missionId);

    if (!mission || mission.status === 'completed') return;

    const updatedMissions = missions.map((m) =>
      m.id === missionId ? { ...m, status: 'completed' } : m
    );

    const updatedUser = {
      ...user,
      experience: user.experience + mission.experienceReward,
      mana: user.mana + mission.manaReward,
    };

    if (mission.skills) {
      mission.skills.forEach((skillReward) => {
        const skillIndex = updatedUser.skills.findIndex(
          (s) => s.id === skillReward.skillId
        );
        if (skillIndex !== -1) {
          updatedUser.skills[skillIndex].progress += skillReward.value * 10;
          if (updatedUser.skills[skillIndex].progress >= 100) {
            updatedUser.skills[skillIndex].level += 1;
            updatedUser.skills[skillIndex].progress = 0;
          }
        }
      });
    }

    if (updatedUser.experience >= updatedUser.neededExperience) {
      updatedUser.experience -= updatedUser.neededExperience;
      updatedUser.rank = updatedUser.nextRank;
      updatedUser.nextRank = getNextRank(updatedUser.rank);
      updatedUser.neededExperience = Math.floor(updatedUser.neededExperience * 1.5);
    }

    set({
      missions: updatedMissions,
      user: updatedUser,
      completedMissions: [...completedMissions, { ...mission, completedAt: new Date() }],
    });
  },

  purchaseItem: (itemId) => {
    const { storeItems, user, inventory } = get();
    const item = storeItems.find((i) => i.id === itemId);

    if (!item || user.mana < item.price) return false;

    const updatedUser = {
      ...user,
      mana: user.mana - item.price,
    };

    set({
      user: updatedUser,
      inventory: [...inventory, { ...item, purchasedAt: new Date() }],
    });

    return true;
  },

  addMission: (mission) => {
    const { missions } = get();
    const newMission = {
      ...mission,
      id: Math.max(...missions.map((m) => m.id), 0) + 1,
      status: 'available',
    };
    set({ missions: [...missions, newMission] });
  },

  updateMission: (missionId, updates) => {
    const { missions } = get();
    const updatedMissions = missions.map((m) =>
      m.id === missionId ? { ...m, ...updates } : m
    );
    set({ missions: updatedMissions });
  },

  deleteMission: (missionId) => {
    const { missions } = get();
    set({ missions: missions.filter((m) => m.id !== missionId) });
  },
}));

function getNextRank(currentRank) {
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
  const currentIndex = ranks.indexOf(currentRank);
  return currentIndex < ranks.length - 1 ? ranks[currentIndex + 1] : currentRank;
}

export default useStore;