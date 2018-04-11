using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemButton : MonoBehaviour
{
    [SerializeField] private UITexture m_texThumbnail = null;
    [SerializeField] private UISprite m_sprBlackCover = null;
    [SerializeField] private UILabel m_lbName = null;

    [HideInInspector] public GameItemHandler onClicked;
    private GameItem m_GameItem;

    public void SetData(GameItem gameItem)
    {
        m_GameItem = gameItem;

        if (gameItem == null)
        {
            //  Show empty button

            m_lbName.text = "";

            return;
        }

        MasterData.GameItem masterData = null;
        MasterDataManager.Instance.GetData<MasterData.GameItem>(gameItem.GetMasterDataID(), ref masterData);

        m_lbName.text = masterData.m_strName;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

#region Event Handler
    [SerializeField] private void OnButtonClicked()
    {
        if (onClicked != null && m_GameItem != null)
        {
            onClicked(m_GameItem);
        }
    }
#endregion
}