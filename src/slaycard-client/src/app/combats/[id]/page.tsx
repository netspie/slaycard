"use client";

import { ActionCard, ActionCardRef } from "@/app/components/ActionCard";
import { Card, CardProps, CardRef } from "@/app/components/Card";
import { useCombatState } from "@/app/state/CombatState";
import { useEffect, useState } from "react";

const cardsData: string[] = [
  "cards/card-1.jpg",
  "cards/card-2.jpg",
  "cards/card-3.jpg",
  "cards/card-4.jpg",
  "cards/card-5.jpg",
];

// const actionCardsData: string[] = [
//   "skills/orbs/row-1-column-1.jpg",
//   "skills/orbs/row-1-column-2.jpg",
//   "skills/orbs/row-1-column-3.jpg",
//   "skills/orbs/row-1-column-4.jpg",
//   "skills/orbs/row-2-column-1.jpg",
//   "skills/orbs/row-2-column-2.jpg",
//   "skills/orbs/row-2-column-3.jpg",
//   "skills/orbs/row-2-column-4.jpg",
// ]

const actionCardsData: string[] = [
  "skills/weapons/shield.png",
  "skills/weapons/swords.png",
  "skills/weapons/speelbook.png",
  "skills/weapons/dragon.png",
  "skills/weapons/shield.png",
  "skills/weapons/swords.png",
  "skills/weapons/speelbook.png",
  "skills/weapons/dragon.png",
];

export default function CombatPage() {
  const cards: (CardRef | null)[] = [];
  const actionCards: (ActionCardRef | null)[] = [];

  const {
    turn,
    enemyDeck,
    playerDeck,
    playerTurnState,
    setSelectedCardForActionState,
  } = useCombatState();
  const [_, refreshView] = useState<any>(null);

  useEffect(() => {
    actionCards
      .sort((x) => (x ? x?.props.id : 0))
      .forEach((card, i) => {
        if (!card || !card.el) return;
        const durationMs = 50 * (i + 1);

        card.el.style.opacity = "0";
        card.el.style.left = "0px";
        card.el.style.transitionDuration = `0ms`;
        card.el.style.transitionTimingFunction = "cubic-bezier(0.4, 0, 0.2, 1)";
        card.el.style.transform = "translateX(0px)";

        setTimeout(() => {
          if (!card.el) return;
          card.el.style.opacity = "1";
          card.el.style.left = "500px";
          card.el.style.transitionDuration = `${durationMs}ms`;
          card.el.style.transitionProperty = `transform, opacity`;
          card.el.style.transitionTimingFunction =
            "cubic-bezier(0.4, 0, 0.2, 1)";
          card.el.style.transform = "translateX(-500px)";
        }, durationMs);
      });
  }, [_]);

  const onCardSelected = (props: CardProps) => {
    setSelectedCardForActionState(props);
    refreshView({});
  };

  return (
    <div className="flex w-full h-full justify-center p-4 select-none flex-col">
      <div className="w-full h-[80%] flex flex-col gap-1 py-10">
        <div className="relative h-full flex gap-2 justify-center">
          {enemyDeck.characterCards.map((card) => (
            <Card
              key={card.id}
              id={card.id}
              name={card.name}
              isOfPlayer={false}
              isSelected={false}
              hp={card.hp}
              energy={card.energy}
              attack={card.attack}
              imagePath={`/${cardsData[card.imageIndex]}`}
              ref={(ref) =>
                !cards.find((x) => x?.props.id === card.id) && cards.push(ref)
              }
            />
          ))}
        </div>
        <div className="relative h-full flex gap-1 justify-center"></div>
        <div className="relative h-full flex gap-2 justify-center items-center overflow-clip">
          {playerTurnState.selectedCardForAction &&
            playerDeck.characterCards
              .find((c) => c.id === playerTurnState.selectedCardForAction?.id)
              ?.actionCards?.map((card, i) => (
                <ActionCard
                  key={card.id}
                  id={card.id}
                  name={card.name}
                  index={i}
                  color={card.color}
                  imagePath={`/${actionCardsData[card.imageIndex]}`}
                  targets={cards}
                  ref={(ref) =>
                    !actionCards.find((x) => x?.props.id === card.id) &&
                    actionCards.push(ref)
                  }
                />
              ))}
        </div>
        <div className="relative h-full flex gap-2 justify-center">
          {playerDeck.characterCards.map((card) => (
            <Card
              key={card.id}
              id={card.id}
              name={card.name}
              isOfPlayer={true}
              isSelected={playerTurnState.selectedCardForAction?.id === card.id}
              onSelected={onCardSelected}
              hp={card.hp}
              energy={card.energy}
              attack={card.attack}
              imagePath={`/${cardsData[card.imageIndex]}`}
            />
          ))}
        </div>
      </div>
      {/* <div className="scroll-view relative w-full h-[20%] flex flex-col gap-1 items-center overflow-x-auto">
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>3</strong> to <strong>Imp</strong></span>
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>2</strong> to <strong>Goblin</strong></span>
          <span className="text-xs text-white"><strong>Goblin</strong> heals <strong>3</strong> hp</span>
          <span className="text-xs text-white"><strong>Imp</strong> damaged <strong>1</strong> to <strong>Garmir</strong></span>
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>3</strong> to <strong>Imp</strong></span>
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>2</strong> to <strong>Goblin</strong></span>
          <span className="text-xs text-white"><strong>Goblin</strong> heals <strong>3</strong> hp</span>
          <span className="text-xs text-white"><strong>Imp</strong> damaged <strong>1</strong> to <strong>Garmir</strong></span>
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>3</strong> to <strong>Imp</strong></span>
          <span className="text-xs text-white"><strong>Ellesandra</strong> damaged <strong>2</strong> to <strong>Goblin</strong></span>
          <span className="text-xs text-white"><strong>Goblin</strong> heals <strong>3</strong> hp</span>
          <span className="text-xs text-white"><strong>Imp</strong> damaged <strong>1</strong> to <strong>Garmir</strong></span>
      </div> */}
    </div>
  );
}
