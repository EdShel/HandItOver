import Vue from 'vue'
import VueI18n from 'vue-i18n'
import messages from '@/en'

Vue.use(VueI18n);

export const i18n = new VueI18n({
    locale: 'en', 
    fallbackLocale: 'en',
    messages: messages
});

const loadedLanguages = ['en'];

function setI18nLanguage(lang) {
    i18n.locale = lang;
    document.querySelector('html').setAttribute('lang', lang);
    return lang
}

export function loadLanguageAsync(lang) {
    // If the same language
    if (i18n.locale === lang) {
        return Promise.resolve(setI18nLanguage(lang))
    }

    // If the language was already loaded
    if (loadedLanguages.includes(lang)) {
        return Promise.resolve(setI18nLanguage(lang))
    }

    // If the language hasn't been loaded yet
    return import( `@/${lang}.js`).then(
        messages => {
            i18n.setLocaleMessage(lang, messages)
            loadedLanguages.push(lang)
            return setI18nLanguage(lang)
        }
    )
}