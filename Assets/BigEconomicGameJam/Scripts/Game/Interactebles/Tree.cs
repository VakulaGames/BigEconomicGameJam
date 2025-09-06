using CORE;
using DG.Tweening;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class Tree: BaseInteractable
    {
        [SerializeField] private int _health = 5;
        [SerializeField] private Board[] _boards;
         
        public override void SetAction()
        {
            _health--;

            if (_health <= 0)
            {
                ShowBoards();
                Destroy(this.gameObject);
            }
            else
            {
                Enabled = false;

                Vector3 scale = transform.localScale;
                
                _sequence = _sequence.MakeSequence()
                    .Append(transform.DOScale(scale * 0.9f, 0.15f).SetEase(Ease.InOutSine))
                    .Append(transform.DOScale(scale * 1.05f, 0.15f).SetEase(Ease.InOutSine))
                    .Append(transform.DOScale(scale * 0.95f, 0.15f).SetEase(Ease.InOutSine))
                    .Append(transform.DOScale(scale, 0.15f).SetEase(Ease.InOutSine))
                    .OnComplete(() =>
                    {
                        Enabled = true;
                    });
            }
        }

        private void ShowBoards()
        {
            if (_boards == null || _boards.Length == 0)
                return;

            foreach (var board in _boards)
            {
                board.transform.SetParent(this.transform.parent);
                board.gameObject.SetActive(true);
                
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f), 
                    0f, 
                    Random.Range(-1f, 1f)
                ).normalized;
    
                float forceMagnitude = Random.Range(2f, 4f); 
                board.Rigidbody.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}