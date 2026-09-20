/* Minimal DOM patcher: updates an existing element tree in place to match a new HTML string, so
   periodic refreshes only touch what changed (no flicker, running CSS animations keep running).
   Elements carrying a data-key attribute are matched by key, everything else by position.
   Patches attributes and text only, so it is not meant for regions containing form controls. */

/** Replaces the children of target with html by patching the existing nodes instead of rebuilding them. */
function patchInnerHtml(target, html) {
  const template = document.createElement('template');
  template.innerHTML = html;
  patchChildren(target, template.content);
}

function patchKeyOf(node) {
  return node.nodeType === Node.ELEMENT_NODE ? node.getAttribute('data-key') : null;
}

function patchSameKind(a, b) {
  if (a.nodeType !== b.nodeType) return false;
  return a.nodeType !== Node.ELEMENT_NODE || (a.nodeName === b.nodeName && a.namespaceURI === b.namespaceURI);
}

function patchChildren(parent, source) {
  const keyed = new Map();
  for (const child of parent.childNodes) {
    const key = patchKeyOf(child);
    if (key !== null) keyed.set(key, child);
  }

  // position is the live node that should occupy the current slot; everything from it onward is unclaimed.
  let position = parent.firstChild;

  for (const next of Array.from(source.childNodes)) {
    const key = patchKeyOf(next);
    let node = null;

    if (key !== null) {
      node = keyed.get(key) || null;
      keyed.delete(key);
    } else if (position && patchKeyOf(position) === null && patchSameKind(position, next)) {
      node = position;
    }

    if (node) {
      patchNode(node, next);
    } else {
      node = next;
    }

    if (node === position) {
      position = position.nextSibling;
    } else {
      parent.insertBefore(node, position);
    }
  }

  while (position) {
    const following = position.nextSibling;
    parent.removeChild(position);
    position = following;
  }
}

function patchNode(live, next) {
  if (live.nodeType !== Node.ELEMENT_NODE) {
    if (live.nodeValue !== next.nodeValue) live.nodeValue = next.nodeValue;
    return;
  }

  for (const attr of Array.from(live.attributes)) {
    if (!next.hasAttribute(attr.name)) live.removeAttribute(attr.name);
  }
  for (const attr of Array.from(next.attributes)) {
    if (live.getAttribute(attr.name) !== attr.value) live.setAttribute(attr.name, attr.value);
  }

  patchChildren(live, next);
}
