using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OMTB.UI
{
    public class CounterSliderUI : MonoBehaviour
    {
        [SerializeField]
        Slider slider;

        [SerializeField]
        Text labelMin, labelMax, labelSel;

        [SerializeField]
        Button buttonAll, buttonSelected, buttonCancel;

     
        public static CounterSliderUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Instance.slider.onValueChanged.AddListener(HandleOnValueChanged);
                Instance.Hide(); 
            }
            else
                Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show(int min, int max, UnityAction<int> Confirm, UnityAction Cancel)
        {
            if (gameObject.activeSelf)
                return;

            gameObject.SetActive(true);

            labelMin.text = min.ToString();
            labelMax.text = max.ToString();
            labelSel.text = min.ToString();

            slider.maxValue = max;
            slider.minValue = min;
            slider.value = min;
            buttonAll.onClick.AddListener(() =>
            {
                Confirm(max);
                Hide();
            });

            buttonSelected.onClick.AddListener(() =>
            {
                Confirm((int)slider.value);
                Hide();
            });

            buttonCancel.onClick.AddListener(() =>
            {
                Cancel();
                Hide();
            });

        }

        void HandleOnValueChanged(float value)
        {
            labelSel.text = value.ToString();
        }

        private void Hide()
        {
            buttonAll.onClick.RemoveAllListeners();
            buttonCancel.onClick.RemoveAllListeners();
            buttonSelected.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

      
    }

}
