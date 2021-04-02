export function localWeight(massUnit, kgNumber) {
    if (massUnit === 'kilo') {
        return kgNumber;
    }
    // Pounds
    return kgNumber * 2.20462;
}
