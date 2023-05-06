using System.Collections.Generic;

namespace JuhaKurisu.PopoTools.ColliderSystem
{
    public class ColliderWorld<T>
    {
        public HashSet<RectCollider<T>> checkColliders = new();
        public HashSet<RectCollider<T>> colliders = new();
        public Dictionary<(long, long), HashSet<RectCollider<T>>> collidersMap = new();

        private List<RectCollider<T>> checkRets = new();
        private List<(RectCollider<T> collider, List<RectCollider<T>>)> checkAllRets = new();
        private HashSet<RectCollider<T>> checkedColliders = new();

        public void AddCollider(RectCollider<T> boxCollider)
        {
            // 失敗したら既に存在するオブジェクトのためreturn
            if (!colliders.Add(boxCollider)) return;
            if (boxCollider.check) checkColliders.Add(boxCollider);
            foreach (var position in boxCollider.gridPositions)
            {
                if (!collidersMap.ContainsKey(position))
                {
                    collidersMap[position] = new();
                }

                collidersMap[position].Add(boxCollider);
            }
            boxCollider.isRegistered = true;
            boxCollider.world = this;
        }

        public void RemoveCollider(RectCollider<T> boxCollider)
        {
            // 失敗したらそもそも存在しないオブジェクトのためreturn
            if (!colliders.Remove(boxCollider)) return;
            if (boxCollider.check) checkColliders.Remove(boxCollider);
            foreach (var position in boxCollider.gridPositions)
            {
                if (!collidersMap.ContainsKey(position))
                {
                    collidersMap[position] = new();
                }

                collidersMap[position].Remove(boxCollider);
            }
            boxCollider.isRegistered = false;
        }

        public List<(RectCollider<T> collider, List<RectCollider<T>> otherCollider)> CheckAll()
        {
            checkAllRets.Clear();

            foreach (var collider in checkColliders)
            {
                checkAllRets.Add((collider, Check(collider)));
            }

            return new(checkAllRets);
        }

        public List<RectCollider<T>> Check(RectCollider<T> boxCollider)
        {
            checkRets.Clear();
            checkedColliders.Clear();

            foreach (var position in boxCollider.gridPositions)
            {
                // ないなら返す
                if (!collidersMap.ContainsKey(position))
                    return checkRets;

                // あるなら全て判定してcheckedCollidersに登録
                foreach (var otherCollider in collidersMap[position])
                {
                    // 既にチェックしてあるなら次へ
                    if (checkedColliders.Contains(otherCollider)) continue;

                    //当たってるなら返り値に登録
                    if (boxCollider.Detect(otherCollider))
                        checkRets.Add(otherCollider);

                    checkedColliders.Add(otherCollider);
                }
            }

            return checkRets;
        }
    }
}