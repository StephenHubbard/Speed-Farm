using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogManager : Singleton<EventLogManager>
{
    [SerializeField] private GameObject _eventLogPrefab;

    public void NewEventLog(string eventText) {
        EventLog newEventLog = Instantiate(_eventLogPrefab, transform).GetComponent<EventLog>();
        newEventLog.transform.SetAsFirstSibling();
        newEventLog.UpdateEventText(eventText);
    }
}
