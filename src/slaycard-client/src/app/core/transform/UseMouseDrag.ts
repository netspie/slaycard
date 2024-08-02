import React, {
  ReactNode,
  Ref,
  RefObject,
  TouchEvent,
  createElement,
  useEffect,
  useState,
} from "react";
import nanSafe from "../numbers/NumberFunctions";
import ReactDOM from "react-dom";

export interface DropElement {
  ref: RefObject<HTMLDivElement>;
  onDragEnter?: () => void;
  onDragExit?: () => void;
}

type ElementData = {
  x: number;
  y: number;

  w: number;
  h: number;

  z: number;
};

const DefaultData = {
  x: 0,
  y: 0,
  w: 0,
  h: 0,
  z: 0,
};

const useMouseDrag = (
  ref: RefObject<HTMLDivElement>,
  targets: (DropElement | null)[] = []
) => {
  const [isSelectedActionCard, setSelectedActionCard] = useState(false);
  const [data, setData] = useState<ElementData>(DefaultData);

  const onMouseMove = (ev: MouseEvent) => {
    if (!ref.current) return;

    ref.current.style.position = "fixed";

    ref.current.style.width = `${data.w}px`;
    ref.current.style.height = `${data.h}px`;

    ref.current.style.left = `${ev.clientX + data.x - data.w / 2}px`;
    ref.current.style.top = `${ev.clientY - data.h / 2}px`;

    targets.forEach((target) => {
      var el = target?.ref.current;
      if (!target || !el) return;

      var rect = el.getBoundingClientRect();
      if (
        ev.clientX > rect.left &&
        ev.clientX < rect.left + rect.width &&
        ev.clientY > rect.top &&
        ev.clientY < rect.top + rect.height
      ) {
        target.onDragEnter && target.onDragEnter();
      } else {
        target.onDragExit && target.onDragExit();
      }
    });
  };

  const onMouseUp = () => {
    if (!ref.current) return;

    window.removeEventListener("mousemove", onMouseMove);
    ref.current.removeEventListener("mouseup", onMouseUp);

    ref.current.style.position = "relative";

    ref.current.style.left = `${data.x}px`;
    ref.current.style.top = "0";

    ref.current.style.zIndex = `${data.z}`;

    setSelectedActionCard(false);
  };

  const onMouseDown = () => {
    if (!ref.current) return;

    setData({
      x: nanSafe(parseInt(ref.current.style.left)),
      y: nanSafe(parseInt(ref.current.style.top)),
      w: ref.current.clientWidth,
      h: ref.current.clientHeight,
      z: nanSafe(parseInt(ref.current.style.zIndex)),
    });

    ref.current.style.zIndex = "1000";

    setSelectedActionCard(true);
  };

  const onTouchMove = (ev: TouchEvent<HTMLDivElement>) => {
    //drawLine(ev.changedTouches[0].clientX, ev.changedTouches[0].clientY);
  };

  useEffect(() => {
    if (!ref.current) return;

    ref.current.addEventListener("mousedown", onMouseDown);
    if (!isSelectedActionCard) return;

    window.addEventListener("mousemove", onMouseMove);
    ref.current.addEventListener("mouseup", onMouseUp);
  }, [isSelectedActionCard]);
};

export default useMouseDrag;
