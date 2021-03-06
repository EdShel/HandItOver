export default {
    units: {
        massUnit: 'kilo',
        mass: '{0}кг'
    },
    page: {
        fallback: 'Hand It Over',
        main: 'Hand It Over',
        mailbox: 'Скрині',
        admin: 'Панель адміністратора',
        users: 'Користувачі',
        group: 'Налаштування групи',
        rentBox: 'Оренда скрині',
        rent: 'Перегляд оренди',
        account: 'Акаунт',
        myAccount: 'Мій акаунт',
        delivery: 'Доставка',
        join: 'Приєднатння до білого списку',
        404: 'Не знайдено',
        403: 'Немає доступу',
    },
    main: {
        descr: "Програмна система для оптимізації процесів, пов'язаних з передачею посилок отримувачам.",
        rentSearchHeader: "Шукати скрині для оренди",
        rentSearchDescr: "Ви можете знайти скриню для доставки ваших пакунків. Просто знайдіть її за назвою, адресою або власником.",
        banner1: "Кладіть та забирайте у будь-який час",
        banner2: "Отримуйте сповіщення про надходження посилок",
        banner3: "Орендуйте публічні скрині для своїх пакунків",
        banner4: "Захист від крадіжок та багато іншого...",
    },
    admin: {
        header: 'Панель адміністратора',
        sslExpire: 'Сертифікат безпеки дійсний до',
        sysConfigHeader: 'Налаштування системи',
        saveConfig: 'Зберегти налаштування',
        downloadConfig: 'Завантажити налаштування',
        backupDbHeader: 'Назва файлу резервної копії (із розширенням)',
        makeBackup: 'Зробити резервну копію',
        restoreBackup: 'Відновити резервну копію',
        downloadBackup: 'Завантажити резервну копію',
        usersHeader: 'Користувачі системи',
        searchUser: 'Шукати за ім\'ям або поштою.',
    },
    users: {
        fullName: 'Ім\'я',
        email: 'Пошта',
        viewUser: 'Переглянути користувача',
        password: 'Пароль',
        repeatPassword: 'Повторіть пароль'
    },
    common: {
        login: 'Вхід',
        register: 'Реєстрація',
        adminPanel: 'Панель адміністратора',
        systemUsers: 'Користувачі',
        myAccount: 'Мій акаунт',
        myMailboxes: 'Мої скрині',
        logout: 'Вийти',
        regiserAction: 'Створити акаунт',
        loginButton: 'Увійти',
        closeAction: 'Закрити',
        cancelAction: 'Відмінити',
        removeAction: 'Видалити',
        addAction: 'Додати',
        configureAction: 'Налаштувати',
        deleteAction: 'Видалити',
        invalidEmailOrPassword: 'Неправильна пошта або пароль.',
        invalidEmail: 'Неправильна пошта.',
        invalidPassword: 'Пароль має містити 6-20 символів.',
        passwordNotSame: 'Користувач із заданою поштою уже зареєстрований.',
        userRegisterd: 'Паролі не співпадають.'
    },
    controls: {
        hours: 'Години',
        minutes: 'Хвилини'
    },
    delivery: {
        giveAwayDescr: 'Введіть ім\'я або пошто нового отримувача та виберіть із списку нижче.',
        deliveryHeader: 'Перегляд доставки',
        addressee: 'Адресат',
        mailboxAddress: 'Адреса скрині',
        weight: 'Вага',
        arrived: 'Прибула',
        terminalTime: 'Термін зберігання',
        taken: 'Одержана',
        predictedTaken: 'Передбачений час одержання',
        giveAwayDelivery: 'Передача посилки',
        giveAwayAction: 'Передати',
        mailboxOwner: 'власник скрині',
        notLimited: 'Не обемежений',
        notTaken: 'Не одержена'
    },
    errors: {
        notFound: '404 Не знайдено',
        noAccess: '403 Немає доступу'
    },
    groups: {
        addToWhitelistHeader: 'Додавання в білий список',
        findUser: 'Знайдіть користувача, якого необхідно додати до "білого" списку групи скринь.',
        groupName: 'Назва групи',
        whitelistOnly: 'Тільки користувачі із "білого" списку можуть орендувати',
        maxRentHours: 'Максимально годин оренди',
        saveChangesAction: 'Зберегти зміни',
        edit: 'Редагувати',
        rents: 'Оренди',
        whitelist: 'Білий список',
        joinLinks: 'Посилання на приєднання',
        whitelistedPeople: 'Користувачі з "білого" списку',
        addUserToWh: 'Додати користувача до "білого" списку',
        genJoinTokenAction: 'Згенерувати нове посилання на приєднання',
        joinLink: 'Посилання на приєднання',
        qrCode: 'QR-код',
        clickToFollowAction: 'Натисність для переходу',
        downloadPng: 'Завантажити як PNG'
    },
    joinGroup: {
        clickToJoinAction: 'Натисність для приєднання',
        waiting: 'Очікуйте',
        nowCanRent: 'Тепер ви можете орендувати скрині даної групи.',
        cantJoin: 'Вибачте, неправильне посилання або Ви вже належите до "білого" списку.'
    },
    rent: {
        rents: 'Оренди',
        from: 'Від',
        until: 'До',
        renter: 'Орендар',
        viewRentAction: 'Переглянути оренду',
        rentOptions: 'Параметри оренди',
        packageSize: 'Розмір пакунку',
        small: 'Малий',
        medium: 'Середній',
        large: 'Великий',
        rentDuration: 'Період оренди',
        minutes: 'хвилин',
        rentTime: 'Час оренди',
        rent: 'Оренда',
        viewingRent: 'Перегляд оренди',
        startTime: 'Початок',
        endTime: 'Кінець',
        cancelRentAction: 'Скасувати оренду',
        vacantIntervals: 'Вільні проміжки часу за день',
        begin: 'Початок',
        end: 'Кінець',
        wrongTime: 'Немає вільних скринь на цей час',
        noRights: 'Немає прав орендувати скриню'
    },
    mailboxes: {
        mailbox: 'Скриня',
        addToGroup: 'Додати до групи',
        chooseGroup: 'Оберіть групу',
        noGroups: 'Потрібно створити хоча б одну групу.',
        editMailbox: 'Редагувати скриню',
        unique: 'унікальна',
        exampleGroupName: 'Наприклад, Харків, АТБ #2',
        addToNewGroup: 'Додати до нової групи',
        addToExistingGroup: 'Додати до існуючої групи',
        removeFromGroupAction: 'Прибрати з групи',
        recentDeliveries: 'Останні посилки',
        predictedTaking: 'Передбачений час',
        mailboxSize: 'Розмір скрині'
    },
    account: {
        currentDeliveries: 'Поточні посилки',
        viewDeliveryAction: 'Переглянути посилку'
    },
    months: [
        "Січ",
        "Лют",
        "Бер",
        "Квіт",
        "Трав",
        "Черв",
        "Лип",
        "Серп",
        "Вер",
        "Жовт",
        "Лист",
        "Груд",
    ],
    daysOfWeek: ["Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"]
};