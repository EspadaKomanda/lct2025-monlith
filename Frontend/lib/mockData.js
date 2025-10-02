export const mockUsers = [
  {
    id: 1,
    name: 'Иван Космонавтов',
    rank: 'Искатель',
    nextRank: 'Пилот-кандидат',
    experience: 250,
    neededExperience: 500,
    mana: 150,
    skills: [
      { id: 1, name: 'Аналитика', level: 2, progress: 60 },
      { id: 2, name: 'Общение', level: 1, progress: 30 },
      { id: 3, name: 'Креативность', level: 1, progress: 45 },
    ],
  },
  {
    id: 2,
    name: 'Мария Звездная',
    rank: 'Пилот-кандидат',
    nextRank: 'Младший пилот',
    experience: 650,
    neededExperience: 750,
    mana: 320,
    skills: [
      { id: 1, name: 'Аналитика', level: 3, progress: 80 },
      { id: 2, name: 'Общение', level: 2, progress: 50 },
      { id: 3, name: 'Креативность', level: 2, progress: 20 },
    ],
  },
  {
    id: 3,
    name: 'Алексей Галактикин',
    rank: 'Младший пилот',
    nextRank: 'Пилот',
    experience: 1100,
    neededExperience: 1125,
    mana: 580,
    skills: [
      { id: 1, name: 'Аналитика', level: 4, progress: 30 },
      { id: 2, name: 'Общение', level: 3, progress: 70 },
      { id: 3, name: 'Креативность', level: 3, progress: 55 },
    ],
  },
];

export const mockMissions = [
  {
    id: 1,
    title: 'Загрузить резюме',
    description:
      'Загрузите ваше актуальное резюме в систему для дальнейшего рассмотрения. Это поможет HR команде лучше понять ваш профессиональный путь.',
    category: 'Рекрутинг',
    experienceReward: 100,
    manaReward: 50,
    skills: [{ skillId: 1, value: 1 }],
    status: 'available',
    requiredRank: 'Искатель',
    icon: 'FileText',
  },
  {
    id: 2,
    title: 'Пройти первичное собеседование',
    description:
      'Познакомьтесь с командой и расскажите о себе. Это возможность показать свои навыки коммуникации и узнать больше о компании.',
    category: 'Рекрутинг',
    experienceReward: 200,
    manaReward: 100,
    skills: [{ skillId: 2, value: 2 }],
    status: 'available',
    requiredRank: 'Искатель',
    icon: 'Users',
  },
  {
    id: 3,
    title: 'Пройти техническое интервью',
    description:
      'Продемонстрируйте свои технические навыки и покажите, как вы решаете задачи. Подготовьте портфолио и будьте готовы к практическим вопросам.',
    category: 'Рекрутинг',
    experienceReward: 300,
    manaReward: 150,
    skills: [
      { skillId: 1, value: 3 },
      { skillId: 3, value: 2 },
    ],
    status: 'available',
    requiredRank: 'Пилот-кандидат',
    icon: 'Code',
  },
  {
    id: 4,
    title: 'Посетить лекцию "Космос начинается здесь"',
    description:
      'Присоединяйтесь к вдохновляющей лекции о том, как мы строим будущее. Узнайте о миссии компании и ключевых проектах.',
    category: 'Лекторий',
    experienceReward: 150,
    manaReward: 75,
    skills: [{ skillId: 2, value: 1 }],
    status: 'available',
    requiredRank: 'Искатель',
    icon: 'Presentation',
  },
  {
    id: 5,
    title: 'Изучить материалы по архитектуре систем',
    description:
      'Пройдите обучающий курс по современным подходам к проектированию систем. Получите базовые знания для работы с нашими проектами.',
    category: 'Лекторий',
    experienceReward: 180,
    manaReward: 90,
    skills: [{ skillId: 1, value: 2 }],
    status: 'available',
    requiredRank: 'Искатель',
    icon: 'BookOpen',
  },
  {
    id: 6,
    title: 'Участвовать в мастер-классе по дизайн-мышлению',
    description:
      'Развивайте креативное мышление и учитесь находить нестандартные решения. Практический опыт работы в команде над реальными кейсами.',
    category: 'Лекторий',
    experienceReward: 220,
    manaReward: 110,
    skills: [
      { skillId: 3, value: 3 },
      { skillId: 2, value: 1 },
    ],
    status: 'available',
    requiredRank: 'Пилот-кандидат',
    icon: 'Lightbulb',
  },
  {
    id: 7,
    title: 'Завершить онбординг',
    description:
      'Пройдите все этапы введения в должность: изучите корпоративные стандарты, познакомьтесь с командой и настройте рабочее окружение.',
    category: 'Развитие',
    experienceReward: 250,
    manaReward: 125,
    skills: [
      { skillId: 1, value: 1 },
      { skillId: 2, value: 2 },
    ],
    status: 'available',
    requiredRank: 'Младший пилот',
    icon: 'Rocket',
  },
  {
    id: 8,
    title: 'Предложить улучшение процесса',
    description:
      'Проанализируйте существующие процессы и предложите идею по их улучшению. Проявите инициативу и помогите компании стать лучше.',
    category: 'Развитие',
    experienceReward: 350,
    manaReward: 175,
    skills: [
      { skillId: 1, value: 3 },
      { skillId: 3, value: 2 },
    ],
    status: 'available',
    requiredRank: 'Пилот',
    icon: 'TrendingUp',
  },
];

export const mockStoreItems = [
  {
    id: 1,
    name: 'Футболка Алабуга Space',
    price: 100,
    image: 'https://images.pexels.com/photos/8532616/pexels-photo-8532616.jpeg',
    category: 'Мерч',
    description: 'Стильная футболка с логотипом космической программы',
  },
  {
    id: 2,
    name: 'Кружка "Космонавт"',
    price: 80,
    image: 'https://images.pexels.com/photos/1251175/pexels-photo-1251175.jpeg',
    category: 'Мерч',
    description: 'Термокружка для настоящих космических первопроходцев',
  },
  {
    id: 3,
    name: 'Худи с звездным небом',
    price: 250,
    image: 'https://images.pexels.com/photos/8532554/pexels-photo-8532554.jpeg',
    category: 'Мерч',
    description: 'Теплое худи с принтом космического неба',
  },
  {
    id: 4,
    name: 'Блокнот "Бортовой журнал"',
    price: 60,
    image: 'https://images.pexels.com/photos/3059748/pexels-photo-3059748.jpeg',
    category: 'Аксессуары',
    description: 'Записывайте свои идеи как настоящий космонавт',
  },
  {
    id: 5,
    name: 'Стикерпак "Космос"',
    price: 40,
    image: 'https://images.pexels.com/photos/1202723/pexels-photo-1202723.jpeg',
    category: 'Аксессуары',
    description: 'Набор космических стикеров для ноутбука',
  },
  {
    id: 6,
    name: 'Рюкзак Galaxy',
    price: 350,
    image: 'https://images.pexels.com/photos/2905238/pexels-photo-2905238.jpeg',
    category: 'Аксессуары',
    description: 'Вместительный рюкзак с космическим дизайном',
  },
  {
    id: 7,
    name: 'Дополнительный выходной',
    price: 500,
    image: 'https://images.pexels.com/photos/1178498/pexels-photo-1178498.jpeg',
    category: 'Привилегии',
    description: 'Заслуженный отдых для настоящего космонавта',
  },
  {
    id: 8,
    name: 'Обед с CEO',
    price: 800,
    image: 'https://images.pexels.com/photos/3184183/pexels-photo-3184183.jpeg',
    category: 'Привилегии',
    description: 'Персональная встреча и обед с руководителем компании',
  },
];

export const categories = ['Все', 'Рекрутинг', 'Лекторий', 'Развитие'];

export const storeCategories = ['Все', 'Мерч', 'Аксессуары', 'Привилегии'];

export const ranks = [
  'Искатель',
  'Пилот-кандидат',
  'Младший пилот',
  'Пилот',
  'Старший пилот',
  'Капитан',
  'Командир',
  'Адмирал',
];