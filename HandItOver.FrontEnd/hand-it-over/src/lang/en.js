export default {
    units: {
        massUnit: 'pounds',
        mass: '{0}lb'
    },
    page: {
        fallback: 'Hand It Over',
        main: 'Hand It Over',
        mailbox: 'Mailboxes',
        admin: 'Admin panel',
        users: 'System users',
        group: 'Group configuration',
        rentBox: 'Renting',
        rent: 'Rent view',
        account: 'User',
        myAccount: 'My account',
        delivery: 'Delivery',
        join: 'Join rent whitelist',
        404: 'Not Found',
    },
    main: {
        descr: "Program system for automation of processes associated with handing deliveries over to the addressee.",
        rentSearchHeader: "Search mailbox to rent",
        rentSearchDescr: "You can find a smart mailbox to deliver your packages there. Just search by its name, owner or address.",
        banner1: "Put and open in any time",
        banner2: "Get notified about arrival of packages",
        banner3: "Rent public mailboxes for your deliveries",
        banner4: "Anti-theft protection and much more...",
    },
    admin: {
        header: 'Admin Panel',
        sslExpire: 'The SSL certificate will expire at',
        sysConfigHeader: 'System configurations',
        saveConfig: 'Save configurations',
        downloadConfig: 'Download configurations',
        backupDbHeader: 'Backup file name (with extension)',
        makeBackup: 'Make backup',
        downloadBackup: 'Download backup',
        usersHeader: 'Users of the system',
        searchUser: 'Search by name or email',
    },
    users: {
        fullName: 'Full name',
        email: 'Email',
        viewUser: 'View user',
        password: 'Password',
        repeatPassword: 'Repeat password'
    },
    common: {
        login: 'Login',
        register: 'Register',
        adminPanel: 'Admin panel',
        systemUsers: 'System users',
        myAccount: 'My account',
        myMailboxes: 'My mailboxes',
        logout: 'Logout',
        regiserAction: 'Register',
        loginButton: 'Login',
        closeAction: 'Close',
        cancelAction: 'Cancel',
        removeAction: 'Remove',
        addAction: 'Add',
        configureAction: 'Configure',
        deleteAction: 'Delete',
        invalidEmailOrPassword: 'Invalid email or password.',
        invalidEmail: 'Invalid email address.',
        invalidPassword: 'Password must contain 6-20 characters.',
        passwordNotSame: 'The user with the email is already registered.',
        userRegisterd: 'Passwords are not the same.'
    },
    controls: {
        hours: 'Hours',
        minutes: 'Minutes'
    },
    delivery: {
        giveAwayDescr: 'Type in new addressee\'s email or full name and select from the list below.',
        deliveryHeader: 'Viewing delivery',
        addressee: 'Addressee',
        mailboxAddress: 'Mailbox address',
        weight: 'Weight',
        arrived: 'Arrived',
        terminalTime: 'Terminal time',
        taken: 'Taken',
        predictedTaken: 'Predicted to be taken',
        giveAwayDelivery: 'Give away delivery',
        giveAwayAction: 'Give away',
        mailboxOwner: 'mailbox owner',
        notLimited: 'Not limited',
        notTaken: 'Not taken'
    },
    errors: {
        notFound: '404 Not Found'
    },
    groups: {
        addToWhitelistHeader: 'Add user to whitelist',
        findUser: 'Find a user to add him to the whitelist.',
        groupName: 'Group name',
        whitelistOnly: 'Only whitelisted users can rent',
        maxRentHours: 'Max rent hours',
        saveChangesAction: 'Save changes',
        edit: 'Edit',
        rents: 'Manage rents',
        whitelist: 'Whitelist',
        joinLinks: 'Join links',
        whitelistedPeople: 'Whitelisted people',
        addUserToWh: 'Add a user to the whitelist',
        genJoinTokenAction: 'Generate new join link',
        joinLink: 'Join link',
        qrCode: 'QR code',
        clickToFollowAction: 'Click to follow',
        downloadPng: 'Download PNG'
    },
    joinGroup: {
        clickToJoinAction: 'Click to join',
        waiting: 'Waiting',
        nowCanRent: 'Now you can rent this group.',
        cantJoin: 'Sorry, invalid token or you already belong to the whitelist.'
    },
    rent: {
        rents: 'Rents',
        from: 'From',
        until: 'Until',
        renter: 'Renter',
        viewRentAction: 'View rent',
        rentOptions: 'Rent options',
        packageSize: 'Package size',
        small: 'Small',
        medium: 'Medium',
        large: 'Large',
        rentDuration: 'Rent duration',
        minutes: 'minutes',
        rentStart: 'Rent start',
        rent: 'Rent',
        viewingRent: 'Viewing rent',
        startTime: 'Start time',
        endTime: 'End time',
        cancelRentAction: 'Cancel rent',
        vacantIntervals: 'Vacant intervals of the day',
        begin: 'Begin',
        end: 'End'
    },
    mailboxes: {
        mailbox: 'Mailbox',
        addToGroup: 'Add to group',
        chooseGroup: 'Choose your group',
        noGroups: 'Create at least one group.',
        editMailbox: 'Edit mailbox',
        unique: 'unique',
        exampleGroupName: 'E.g. Kharkiv ATB Market #2',
        addToNewGroup: 'Add to new group',
        addToExistingGroup: 'Add to existing group',
        removeFromGroupAction: 'Remove from group',
        recentDeliveries: 'Recent deliveries',
        predictedTaking: 'Predicted taking',
        mailboxSize: 'Mailbox size'
    },
    account: {
        currentDeliveries: 'Current deliveries',
        viewDeliveryAction: 'View delivery'
    }
};