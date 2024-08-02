
export type Coords = {
  x: number;
  y: number;
  x1: number;
  y1: number;
};

export type Line = {
  color: string;
  width: number;
};

export function createLine(
  canvas: HTMLCanvasElement,
  coords: Coords,
  line: Line = { color: "red", width: 1 }
) {

  canvas.width = canvas.clientWidth;
  canvas.height = canvas.clientHeight;
  canvas.style.top = '0';
  canvas.style.left = '0';

  console.log(canvas.width + 'x' + canvas.height)
  const ct = canvas.getContext("2d");
  if (!ct) return;

  drawLine(ct, coords, line);
}

export function createShadow(
  canvas: HTMLCanvasElement,
  coords: Coords,
  line: Line = { color: "red", width: 1 }
) {
  canvas.width = canvas.clientWidth;
  canvas.height = canvas.clientHeight;
  canvas.style.top = '0';
  canvas.style.left = '0';
  const ct = canvas.getContext("2d");
  if (!ct) return;

  drawShadow(ct, coords, line);
}

export function clearCanvas(canvas: HTMLCanvasElement) {
  canvas?.getContext("2d")?.clearRect(0, 0, canvas.width, canvas.height);
}

function drawLine(ct: CanvasRenderingContext2D, coords: Coords, line: Line) {
  ct.beginPath();
  ct.moveTo(coords.x, coords.y);
  ct.lineTo(coords.x1, coords.y1);
  ct.strokeStyle = line.color;
  ct.lineWidth = line.width;
  ct.stroke();
}

function drawShadow(ct: CanvasRenderingContext2D, coords: Coords, line: Line) {
  const offset = 100000

  ct.beginPath();
  ct.moveTo(coords.x - offset, coords.y - offset);
  ct.lineTo(coords.x1 - offset, coords.y1 - offset);
  ct.strokeStyle = line.color;
  ct.lineWidth = 5;
  ct.shadowBlur = 3;
  ct.shadowColor = "rgba(0, 0, 0, 1)"
  ct.shadowOffsetX = offset
  ct.shadowOffsetY = offset
  ct.stroke();

  ct.beginPath();
  ct.moveTo(coords.x - offset, coords.y - offset);
  ct.lineTo(coords.x1 - offset, coords.y1 - offset);
  ct.strokeStyle = line.color;
  ct.lineWidth = 5;
  ct.shadowBlur = 3;
  ct.shadowColor = "rgba(0, 0, 0, 1)"
  ct.shadowOffsetX = offset
  ct.shadowOffsetY = offset
  ct.stroke();
}
