import { i18n } from '~/i18n'


export function localWeight(massUnit, kgNumber) {
    if (massUnit === 'kilo') {
        return kgNumber;
    }
    return kgNumber * 0.45359237;
}
