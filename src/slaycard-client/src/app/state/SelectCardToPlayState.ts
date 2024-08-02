import { CardProps } from "@/app/components/Card";
import { create } from "zustand";

type SelectCardToPlayState = {
  isSelected_CardToPlay: boolean;
  selected_CardToPlay: CardProps | undefined;
  setSelected_CardToPlay: (
    isSelected: boolean,
    selectedCard: CardProps | undefined
  ) => void;
};

export const useSelectCardToPlayState = create<SelectCardToPlayState>()(
  (set) => ({
    isSelected_CardToPlay: false,
    selected_CardToPlay: undefined,
    setSelected_CardToPlay: (
      isSelected: boolean,
      selectedCard: CardProps | undefined
    ) =>
      set({
        isSelected_CardToPlay: isSelected,
        selected_CardToPlay: selectedCard,
      }),
  })
);
