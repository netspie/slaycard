
export default function nanSafe(n: number, or: number = 0): number {
    if (Number.isNaN(n))
        return or;

    return n;
}
