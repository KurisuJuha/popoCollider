using System.Collections.Generic;

namespace JuhaKurisu.PopoTools.ColliderSystem
{
    public class ColliderWorld
    {
        public HashSet<BoxCollider> checkColliders = new();
        public HashSet<BoxCollider> colliders = new();
        public Dictionary<(long, long), HashSet<BoxCollider>> collidersMap = new();

        private List<BoxCollider> checkRets = new();
        private List<(BoxCollider collider, List<BoxCollider>)> checkAllRets = new();
        private HashSet<BoxCollider> checkedColliders = new();

        public void AddCollider(BoxCollider boxCollider)
        {
            colliders.Add(boxCollider);
            if (boxCollider.check) checkColliders.Add(boxCollider);
            var positions = boxCollider.gridPositions;
            foreach (var position in positions)
            {
                if (!collidersMap.ContainsKey(position))
                {
                    collidersMap[position] = new();
                }

                collidersMap[position].Add(boxCollider);
            }
        }

        public void RemoveCollider(BoxCollider boxCollider)
        {
            colliders.Remove(boxCollider);
            if (boxCollider.check) checkColliders.Remove(boxCollider);
            var positions = boxCollider.gridPositions;
            foreach (var position in positions)
            {
                if (!collidersMap.ContainsKey(position))
                {
                    collidersMap[position] = new();
                }

                collidersMap[position].Remove(boxCollider);
            }
        }

        public List<(BoxCollider collider, List<BoxCollider> otherCollider)> CheckAll()
        {
            checkAllRets.Clear();

            foreach (var collider in checkColliders)
            {
                checkAllRets.Add((collider, Check(collider)));
            }

            return checkAllRets;
        }

        public List<BoxCollider> Check(BoxCollider boxCollider)
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